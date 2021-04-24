using System;
using System.Collections.Generic;

namespace wpftdd.routes
{
    public class Routes
    {
        private readonly HashSet<UriRoute> _uriRoutes = new();
        private readonly HashSet<NamedRoute> _namedRoutes = new();

        public void Add(NamedRoute route) => _namedRoutes.Add(route);

        public void Add(UriRoute route) => _uriRoutes.Add(route);
    }
}