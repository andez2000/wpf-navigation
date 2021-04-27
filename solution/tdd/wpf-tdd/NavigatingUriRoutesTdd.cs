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
    public sealed class NavigatingUriRoutesTdd
    {
        //private readonly UriRoute _page1Route = new(new Uri("/Pages/Page1.xaml", UriKind.Absolute));

        private readonly RouteNavigationService _routeNavigationService;
        private readonly IServiceProvider _serviceProvider;
        private (Thread Thread, MainWindow mainWindow) _context;

        public NavigatingUriRoutesTdd()
        {
            _context = new(null, null);

            var routes = new NamedRoutes();
            var views = new Views();

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
        public void Can_navigate_to_uri_page()
        {
            using (_serviceProvider.CreateScope())
            {
                Uri uri = new Uri("wpf-tdd;component/WpfApp/Pages/Page1.xaml", UriKind.RelativeOrAbsolute);
                Uri expectedUri = new Uri("wpf-tdd;component/WpfApp/Pages/Page1.xaml", UriKind.RelativeOrAbsolute);
                
                var runTestMonitor = new ManualResetEventSlim(false);

                _context = WindowDispatch.CreateWindowOnSTAThread(() => new MainWindow(), w => { });

                WindowDispatch.DispatchOn(
                    _context.mainWindow, () => _routeNavigationService.NavigateTo(uri),
                    runTestMonitor, TimeSpan.FromSeconds(3));

                Uri sourceUri = WindowDispatch.GetProperty(_context.mainWindow, () => _context.mainWindow.NavigationHost.Source);
                object content = WindowDispatch.GetProperty(_context.mainWindow, () => _context.mainWindow.NavigationHost.Content);
                
                Assert.True(
                    Uri.Compare(
                        sourceUri,
                        expectedUri,
                        UriComponents.AbsoluteUri, 
                        UriFormat.UriEscaped, 
                        StringComparison.InvariantCultureIgnoreCase) == 0
                );
                
                Assert.Equal(typeof(Page1), content.GetType());

                WindowDispatch.DispatchOnWithWait(
                    _context.mainWindow, () => _context.mainWindow.Close());
            }
        }
    }
}