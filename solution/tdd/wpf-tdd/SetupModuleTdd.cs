using System;
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
        private readonly NamedRoute page1Route = new NamedRoute("Page1", typeof(Page1));
        private readonly NamedRoute page2Route = new NamedRoute("Page2", typeof(Page2WithVm));
        private readonly NamedRoute page3Route = new NamedRoute("Page3", typeof(Page3WithVm));
        private IServiceProvider serviceProvider;
        
        [UIFact]
        public void TddIt()
        {
            // configuration
            Routes routes = new Routes();
            routes.AddAll(page1Route, page2Route, page3Route);

            Views views = new Views();
            views.Register<Page2WithVm, Page2Vm>((view, dataContext) => view.DataContext = dataContext);
            views.RegisterForAutoDataContext<Page3WithVm, Page3Vm>();
            // what to do with page1

            // resolution
            NamedRouteResolver namedRouteResolver = new(routes);
            RouteResolver routeResolver = new(namedRouteResolver);
            ViewResolver viewResolver = new ViewResolver(views, type => serviceProvider.GetService(type));
            RouteNavigationService routeNavigationService = new(routeResolver, viewResolver);

            // add all routes to the container...  what is the scope for each view :?
            // wire up for DI
            ServiceCollection serviceCollection = new();
            serviceCollection.AddTransient<Page1>();
            serviceCollection.AddTransient<Page2WithVm>();
            serviceCollection.AddTransient<Page2Vm>();
            serviceCollection.AddTransient<Page3WithVm>();
            serviceCollection.AddTransient<Page3Vm>();

            ContainerBuilder containerBuilder = new();
            containerBuilder.Populate(serviceCollection);
            IContainer container = containerBuilder.Build();
            serviceProvider = new AutofacServiceProvider(container);

            routeNavigationService.NavigateTo(Named("Page2"));
        }
    }
}