using System;
using System.Threading;
using System.Windows.Navigation;
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

    public class SetupModuleTdd
    {
        // this is wrong
        private readonly NamedRoute _page1Route = new("Page1", typeof(Page1));
        private readonly NamedRoute _page2Route = new("Page2", typeof(Page2WithVm));
        private readonly NamedRoute _page3Route = new("Page3", typeof(Page3WithVm));
        private readonly MainWindow _mainWindow = new ();
        
        [UIFact]
        public void TddIt()
        {
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
            NavigationController navigationController = new(() => _mainWindow.NavigationHost.NavigationService);
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
                _mainWindow.NavigationHost.NavigationService.NavigationProgress += NavigationServiceOnNavigationProgress;
                routeNavigationService.NavigateTo(Named("Page2"));
                Thread.Sleep(20000);
                Assert.Equal(_mainWindow.NavigationHost.Content, serviceProvider.GetService<Page2Vm>());
                
                routeNavigationService.NavigateTo(Named("Page3"));
                Thread.Sleep(2000);
                Assert.Equal(_mainWindow.NavigationHost.Content, serviceProvider.GetService<Page3Vm>());
            }
        }

        private void NavigationServiceOnNavigationProgress(object sender, NavigationProgressEventArgs e)
        {
        }
    }
}