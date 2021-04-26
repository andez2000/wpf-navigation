using System;
using System.Linq;

namespace acme.wpftdd.routes
{
    public class NamedRouteResolver
    {
        private readonly Routes _routes;

        public NamedRouteResolver(Routes routes) => 
            _routes = 
                routes ?? throw new ArgumentNullException(nameof(routes));

        public Type Resolve(Name routeName) => 
            _routes.NamedRoutes.FirstOrDefault(r => r.Name == routeName)?.ViewType;
    }
}