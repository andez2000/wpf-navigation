using System;
using System.Threading;
using acme.wpftdd.routes;
using acme.wpftdd.views;
using acme.wpftdd.WpfApp.Pages;
using acme.wpftdd.WpfApp.ViewModels;
using acme.wpftdd.WpfApp.Windows;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;
using Xunit;
using static acme.wpftdd.routes.Name;

namespace acme.wpftdd
{
    // https://autofaccn.readthedocs.io/en/latest/integration/netcore.html
    // https://stackoverflow.com/questions/13381967/show-wpf-window-from-test-unit
    public sealed class SetupModuleTdd
    {
        private readonly NamedRoute _page1Route = new("Page1", typeof(Page1));
        private readonly NamedRoute _page2Route = new("Page2", typeof(Page2WithVm));
        private readonly NamedRoute _page3Route = new("Page3", typeof(Page3WithVm));

        private readonly RouteNavigationService _routeNavigationService;
        private readonly IServiceProvider _serviceProvider;
        private (Thread Thread, MainWindow mainWindow) _context;

        public SetupModuleTdd()
        {
            _context = new(null, null);

            var routes = new Routes();
            routes.AddAll(_page1Route, _page2Route, _page3Route);

            var views = new Views();
            views.Register<Page2WithVm, Page2Vm>((view, dataContext) => view.DataContext = dataContext);
            views.RegisterForAutoDataContext<Page3WithVm, Page3Vm>();

            NamedRouteResolver namedRouteResolver = new(routes);
            RouteResolver routeResolver = new(namedRouteResolver);
            ViewResolver viewResolver = new(views, type => _serviceProvider.GetService(type));
            NavigationController navigationController = new(() => _context.mainWindow.NavigationHost.NavigationService);
            _routeNavigationService = new(routeResolver, viewResolver, navigationController);

            ServiceCollection serviceCollection = new();
            serviceCollection.AddScoped<Page1>();
            serviceCollection.AddScoped<Page2WithVm>();
            serviceCollection.AddScoped<Page2Vm>();
            serviceCollection.AddScoped<Page3WithVm>();
            serviceCollection.AddScoped<Page3Vm>();

            ContainerBuilder containerBuilder = new();
            containerBuilder.Populate(serviceCollection);
            IContainer container = containerBuilder.Build();
            _serviceProvider = new AutofacServiceProvider(container);
        }

        [UIFact]
        public void Can_navigate_to_multiple_named_pages()
        {
            using (_serviceProvider.CreateScope())
            {
                var runTestMonitor = new ManualResetEventSlim(false);

                _context = WindowDispatch.CreateWindowOnSTAThread(() => new MainWindow(), w => { });

                WindowDispatch.DispatchOn(
                    _context.mainWindow, () => _routeNavigationService.NavigateTo(Named("Page2")),
                    runTestMonitor, TimeSpan.FromSeconds(3));

                Assert.Equal(
                    WindowDispatch.GetProperty(_context.mainWindow, () => _context.mainWindow.NavigationHost.Content),
                    _serviceProvider.GetService<Page2WithVm>()
                );

                runTestMonitor.Reset();

                WindowDispatch.DispatchOn(
                    _context.mainWindow, () => { _routeNavigationService.NavigateTo(Named("Page3")); }, runTestMonitor,
                    TimeSpan.FromSeconds(1));

                Assert.Equal(
                    WindowDispatch.GetProperty(_context.mainWindow, () => _context.mainWindow.NavigationHost.Content),
                    _serviceProvider.GetService<Page3WithVm>()
                );

                WindowDispatch.DispatchOnWithWait(
                    _context.mainWindow, () => _context.mainWindow.Close());
            }
        }
    }
}