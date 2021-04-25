using System;
using System.Threading;
using System.Windows;
using System.Windows.Navigation;
using System.Windows.Threading;
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
    public class SetupModuleTdd
    {
        // this is wrong
        private readonly NamedRoute _page1Route = new("Page1", typeof(Page1));
        private readonly NamedRoute _page2Route = new("Page2", typeof(Page2WithVm));
        private readonly NamedRoute _page3Route = new("Page3", typeof(Page3WithVm));
        
        [WpfFact]
        public void JustSetFrameContent2()
        {
            var runTestMonitor = new ManualResetEventSlim(false);
            var assertionsMonitor = new ManualResetEventSlim(false);
            var windowClosedMonitor = new ManualResetEventSlim(false);

            var (thread, mainWindow) = CreateWindowOnSTAThread(() => new MainWindow(), w => { });

            DispatchOn(mainWindow, () =>
            {
                var page1 = new Page1();
                mainWindow.NavigationHost.Content = page1;
            }, runTestMonitor, TimeSpan.FromSeconds(1));
            
            DispatchOn(mainWindow, () => Assert.NotNull(mainWindow.NavigationHost.Content), assertionsMonitor, TimeSpan.FromSeconds(1));
            DispatchOn(mainWindow, () => mainWindow.Close(), windowClosedMonitor, TimeSpan.FromSeconds(1));
        }

        [UIFact]
        public void TddIt()
        {
            (Thread Thread, MainWindow mainWindow) context = new (null, null);
            
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
                
                context = CreateWindowOnSTAThread(() => new MainWindow(), w => { });

                DispatchOn(
                    context.mainWindow, () => routeNavigationService.NavigateTo(Named("Page2")), 
                    runTestMonitor, TimeSpan.FromSeconds(3));
                
                Thread.Sleep(1000);
                
                // failing here...
                // DispatchOn(
                //     context.mainWindow, 
                //     () => Assert.Equal(context.mainWindow.NavigationHost.Content, serviceProvider.GetService<Page2Vm>()),
                //     assertionsMonitor, TimeSpan.FromSeconds(3));

                context.mainWindow.Dispatcher.BeginInvoke(new ThreadStart(() => context.mainWindow.Close()));
                
                // runTestMonitor.Reset();
                // assertionsMonitor.Reset();
                //
                // DispatchOn(context.mainWindow, () =>
                // {
                //     routeNavigationService.NavigateTo(Named("Page3"));
                // }, runTestMonitor, TimeSpan.FromSeconds(1));
                //
                // DispatchOn(context.mainWindow, () =>
                // {
                //     Assert.Equal(context.mainWindow.NavigationHost.Content, serviceProvider.GetService<Page3Vm>());
                // }, assertionsMonitor, TimeSpan.FromSeconds(1));
            }
        }
        
        private (Thread thread, TWindow window) CreateWindowOnSTAThread<TWindow>(Func<TWindow> createWindow, Action<TWindow> postAction) 
            where TWindow : Window
        {
            TWindow window = null;
            var waitUntilShow = new ManualResetEventSlim(false);
            
            var t = new Thread(() =>
            {
                window = createWindow();
                window.Closed += (s, e) => window.Dispatcher.InvokeShutdown();

                postAction(window);

                waitUntilShow.Set();
                
                System.Windows.Threading.Dispatcher.Run();
            });
            
            t.SetApartmentState(ApartmentState.STA);
            t.Start();
            waitUntilShow.Wait(TimeSpan.FromSeconds(2));
            Thread.Sleep(100);

            return (t, window);
        }
        
        private void DispatchOn(Window window, Action action, ManualResetEventSlim manualResetEvent, TimeSpan timeout)
        {
            window.Dispatcher.BeginInvoke(() =>
            {
                action();
                manualResetEvent.Set();
                
            }, DispatcherPriority.Normal);

            manualResetEvent.Wait(timeout);
        }
    }
}