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
        
        
        

        // [WpfFact]
        // public void JustSetFrameContent()
        // {
        //     var page1 = new Page1();
        //     _mainWindow.Show();
        //     _mainWindow.NavigationHost.Content = page1;
        //     
        //     System.Windows.Threading.Dispatcher.Run();
        //     
        //     Thread.Sleep(3000);
        //     
        //     Assert.NotNull(_mainWindow.NavigationHost.Content);
        // }

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
            waitUntilShow.Wait(TimeSpan.FromSeconds(1));

            return (t, window);
        }
        
        [WpfFact]
        public void JustSetFrameContent2()
        {
            var showMonitor = new ManualResetEventSlim(false);
            var runTestMonitor = new ManualResetEventSlim(false);
            var assertionsMonitor = new ManualResetEventSlim(false);
            var windowClosedMonitor = new ManualResetEventSlim(false);

            (Thread thread, MainWindow mainWindow) context = CreateWindowOnSTAThread(() => new MainWindow(), _ => { });

//            showMonitor.Wait(TimeSpan.FromSeconds(1));

            context.mainWindow.Dispatcher.BeginInvoke(() =>
            {
                var page1 = new Page1();
                context.mainWindow.NavigationHost.Content = page1;
                runTestMonitor.Set();
                
            }, DispatcherPriority.Normal);

            runTestMonitor.Wait(TimeSpan.FromMinutes(1));

            context.mainWindow.Dispatcher.BeginInvoke(() =>
            {
                Assert.NotNull(context.mainWindow.NavigationHost.Content);
                assertionsMonitor.Set();
                
            }, DispatcherPriority.Normal);

            assertionsMonitor.Wait(TimeSpan.FromSeconds(1));

            DispatchOn(context.mainWindow, () =>
            {
                context.mainWindow.Close();
                windowClosedMonitor.Set();
            });
            
            context.mainWindow.Dispatcher.BeginInvoke(() =>
            {
                context.mainWindow.Close();
                windowClosedMonitor.Set();
                
            }, DispatcherPriority.Normal);


            windowClosedMonitor.Wait(TimeSpan.FromSeconds(1));
        }

        private void DispatchOn(Window window, Action action)
        {
            window.Dispatcher.BeginInvoke(action, DispatcherPriority.Normal);
        }
        
        // [UIFact]
        // public void TddIt()
        // {
        //     // configuration
        //     Routes routes = new Routes();
        //     routes.AddAll(_page1Route, _page2Route, _page3Route);
        //
        //     Views views = new Views();
        //     views.Register<Page2WithVm, Page2Vm>((view, dataContext) => view.DataContext = dataContext);
        //     views.RegisterForAutoDataContext<Page3WithVm, Page3Vm>();
        //     // what to do with page1
        //
        //     // resolution
        //     IServiceProvider serviceProvider = null;
        //     
        //     NamedRouteResolver namedRouteResolver = new(routes);
        //     RouteResolver routeResolver = new(namedRouteResolver);
        //     ViewResolver viewResolver = new ViewResolver(views, type => serviceProvider.GetService(type));
        //     NavigationController navigationController = new(() => _mainWindow.NavigationHost.NavigationService);
        //     RouteNavigationService routeNavigationService = new(routeResolver, viewResolver, navigationController);
        //
        //     // add all routes to the container...  what is the scope for each view :?
        //     // wire up for DI
        //     ServiceCollection serviceCollection = new();
        //     serviceCollection.AddScoped<Page1>();
        //     serviceCollection.AddScoped<Page2WithVm>();
        //     serviceCollection.AddScoped<Page2Vm>();
        //     serviceCollection.AddScoped<Page3WithVm>();
        //     serviceCollection.AddScoped<Page3Vm>();
        //
        //     ContainerBuilder containerBuilder = new();
        //     containerBuilder.Populate(serviceCollection);
        //     IContainer container = containerBuilder.Build();
        //     serviceProvider = new AutofacServiceProvider(container);
        //     using (serviceProvider.CreateScope())
        //     {
        //         _mainWindow.Show();
        //         //_mainWindow.NavigationHost.NavigationService.NavigationProgress += NavigationServiceOnNavigationProgress;
        //         
        //         routeNavigationService.NavigateTo(Named("Page2"));
        //         
        //         _mainWindow.NavigationHost.Content = serviceProvider.GetService<Page2Vm>();
        //         
        //         Assert.NotNull(_mainWindow.NavigationHost.Content); //, serviceProvider.GetService<Page2Vm>());
        //         
        //         // routeNavigationService.NavigateTo(Named("Page3"));
        //         // Thread.Sleep(2000);
        //         // Assert.Equal(_mainWindow.NavigationHost.Content, serviceProvider.GetService<Page3Vm>());
        //     }
        // }

    }
}