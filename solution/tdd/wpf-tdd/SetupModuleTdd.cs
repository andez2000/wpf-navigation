using System;
using System.Threading;
using acme.external.Pages;
using acme.external.ViewModels;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;
using wpftdd.routes;
using wpftdd.views;
using Xunit;
using static wpftdd.routes.Name;

namespace wpftdd
{
    // https://autofaccn.readthedocs.io/en/latest/integration/netcore.html
    // https://stackoverflow.com/questions/13381967/show-wpf-window-from-test-unit
    public sealed class SetupModuleTdd
    {
        // this is wrong
        private readonly NamedRoute _page1Route = new("Page1", typeof(Page1));
        private readonly NamedRoute _page2Route = new("Page2", typeof(Page2WithVm));
        private readonly NamedRoute _page3Route = new("Page3", typeof(Page3WithVm));

        [UIFact]
        public void Can_navigate_to_different_pages()
        {
            (Thread Thread, MainWindow mainWindow) context = new(null, null);

            // configuration
            Routes routes = new Routes();
            routes.AddAll(_page1Route, _page2Route, _page3Route);

            Views views = new Views();
            views.Register<Page2WithVm, Page2Vm>((view, dataContext) => view.DataContext = dataContext);
            views.RegisterForAutoDataContext<Page3WithVm, Page3Vm>();
            // what to do with page1

            // resolution
            IServiceProvider serviceProvider = null;

            NamedRouteResolver namedRouteResolver = new(routes);
            RouteResolver routeResolver = new(namedRouteResolver);
            ViewResolver viewResolver = new ViewResolver(views, type => serviceProvider.GetService(type));
            NavigationController navigationController = new(() => context.mainWindow.NavigationHost.NavigationService);
            RouteNavigationService routeNavigationService = new(routeResolver, viewResolver, navigationController);

            // add all routes to the container...  what is the scope for each view :?
            // wire up for DI
            ServiceCollection serviceCollection = new();
            serviceCollection.AddScoped<Page1>();
            serviceCollection.AddScoped<Page2WithVm>();
            serviceCollection.AddScoped<Page2Vm>();
            serviceCollection.AddScoped<Page3WithVm>();
            serviceCollection.AddScoped<Page3Vm>();

            ContainerBuilder containerBuilder = new();
            containerBuilder.Populate(serviceCollection);
            IContainer container = containerBuilder.Build();
            serviceProvider = new AutofacServiceProvider(container);
            using (serviceProvider.CreateScope())
            {
                var runTestMonitor = new ManualResetEventSlim(false);
                var assertionsMonitor = new ManualResetEventSlim(false);

                context = WindowDispatch.CreateWindowOnSTAThread(() => new MainWindow(), w => { });

                WindowDispatch.DispatchOn(
                    context.mainWindow, () => routeNavigationService.NavigateTo(Named("Page2")),
                    runTestMonitor, TimeSpan.FromSeconds(3));

                Thread.Sleep(1000);

                Assert.Equal(
                    WindowDispatch.GetProperty(context.mainWindow, () => context.mainWindow.NavigationHost.Content),
                    serviceProvider.GetService<Page2WithVm>()
                );

                // we need sleeps between navigation to give ui chance to update :)...
                Thread.Sleep(1000);
                WindowDispatch.DispatchOn(context.mainWindow,
                    () => { routeNavigationService.NavigateTo(Named("Page3")); }, runTestMonitor,
                    TimeSpan.FromSeconds(1));

                Thread.Sleep(1000);


                var property = WindowDispatch.GetProperty(context.mainWindow,
                    () => context.mainWindow.NavigationHost.Content);
                Assert.Equal(
                    property,
                    serviceProvider.GetService<Page3WithVm>()
                );

                context.mainWindow?.Dispatcher.BeginInvoke(new ThreadStart(() => context.mainWindow.Close()));
            }
        }
    }
}