using System;
using System.Linq;

namespace acme.wpftdd.routes
{
    public class NamedRouteResolver
    {
        private readonly NamedRoutes _namedRoutes;

        public NamedRouteResolver(NamedRoutes namedRoutes) => 
            _namedRoutes = 
                namedRoutes ?? throw new ArgumentNullException(nameof(namedRoutes));

        public Type Resolve(Name routeName) => 
            _namedRoutes.All.FirstOrDefault(r => r.Name == routeName)?.ViewType;
    }
}