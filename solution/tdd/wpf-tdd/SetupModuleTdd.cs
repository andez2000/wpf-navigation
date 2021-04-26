﻿using System;
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
            var viewResolver = new ViewResolver(views, type => _serviceProvider.GetService(type));
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
        public void Can_navigate_to_different_pages()
        {
            using (_serviceProvider.CreateScope())
            {
                var runTestMonitor = new ManualResetEventSlim(false);

                _context = WindowDispatch.CreateWindowOnSTAThread(() => new MainWindow(), w => { });

                WindowDispatch.DispatchOn(
                    _context.mainWindow, () => _routeNavigationService.NavigateTo(Named("Page2")),
                    runTestMonitor, TimeSpan.FromSeconds(3));

                Thread.Sleep(1000);

                Assert.Equal(
                    WindowDispatch.GetProperty(_context.mainWindow, () => _context.mainWindow.NavigationHost.Content),
                    _serviceProvider.GetService<Page2WithVm>()
                );

                // we need sleeps between navigation to give ui chance to update :)...
                Thread.Sleep(1000);
                WindowDispatch.DispatchOn(_context.mainWindow,
                    () => { _routeNavigationService.NavigateTo(Named("Page3")); }, runTestMonitor,
                    TimeSpan.FromSeconds(1));

                Thread.Sleep(1000);


                var property = WindowDispatch.GetProperty(_context.mainWindow,
                    () => _context.mainWindow.NavigationHost.Content);
                Assert.Equal(
                    property,
                    _serviceProvider.GetService<Page3WithVm>()
                );

                _context.mainWindow?.Dispatcher.BeginInvoke(new ThreadStart(() => _context.mainWindow.Close()));
            }
        }
    }
}