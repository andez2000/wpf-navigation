using System;
using acme.wpftdd.routes;
using acme.wpftdd.views;

namespace acme.wpftdd
{
    /// <summary>
    /// Provides the service for navigation of routes.
    /// </summary>
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

        /// <summary>
        /// Navigate to the specified named route..
        /// </summary>
        /// <param name="routeName">The named route to navigate to.</param>
        public void NavigateTo(Name routeName)
        {
            Type viewType = _routeResolver.ResolveNamedRouteView(routeName);
            object frameworkElement = _viewResolver.Resolve(viewType);

            _navigationController.NavigateTo(frameworkElement);
        }
        
        /// <summary>
        /// Navigates to a specified Uri.
        /// </summary>
        /// <param name="uri">The uri to navigate to.</param>
        public void NavigateTo(Uri uri)
        {
            _navigationController.NavigateTo(uri);
        }
    }
}