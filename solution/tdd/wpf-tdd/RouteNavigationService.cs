using System;
using acme.wpftdd.routes;
using acme.wpftdd.views;

namespace wpftdd
{
    public class RouteNavigationService
    {
        private readonly RouteResolver _routeResolver;
        private readonly ViewResolver _viewResolver;
        private readonly NavigationController _navigationController;

        public RouteNavigationService(
            RouteResolver routeResolver, 
            ViewResolver viewResolver,
            NavigationController navigationController
            )
        {
            _routeResolver = routeResolver ?? throw new ArgumentNullException(nameof(routeResolver));
            _viewResolver = viewResolver ?? throw new ArgumentNullException(nameof(viewResolver));
            _navigationController = navigationController ?? throw new ArgumentNullException(nameof(navigationController));
        }

        public void NavigateTo(Name routeName)
        {
            Type viewType = _routeResolver.ResolveNamedRouteView(routeName);
            object frameworkElement = _viewResolver.Resolve(viewType);

            _navigationController.NavigateTo(frameworkElement);
        }
    }
}