using System;

namespace acme.wpftdd.routes
{
    public class RouteResolver
    {
        private readonly NamedRouteResolver _namedRouteResolver;

        public RouteResolver(NamedRouteResolver namedRouteResolver) => 
            _namedRouteResolver = 
                namedRouteResolver ?? throw new ArgumentNullException(nameof(namedRouteResolver));

        public Type ResolveNamedRouteView(Name routeName)
        {
            return _namedRouteResolver.Resolve(routeName);
        }
    }
}