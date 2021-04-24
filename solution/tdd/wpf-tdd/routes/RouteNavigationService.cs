using System;
using wpftdd.views;

namespace wpftdd.routes
{
    public class RouteNavigationService
    {
        private readonly RouteResolver _routeResolver;
        private readonly ViewResolver _viewResolver;

        public RouteNavigationService(RouteResolver routeResolver, ViewResolver viewResolver)
        {
            _routeResolver = routeResolver ?? throw new ArgumentNullException(nameof(routeResolver));
            _viewResolver = viewResolver ?? throw new ArgumentNullException(nameof(viewResolver));
        }

        public void NavigateTo(Name routeName)
        {
            Type viewType = _routeResolver.ResolveNamedRouteView(routeName);
            var frameworkElement = _viewResolver.Resolve(viewType);
            
        }
    }
}