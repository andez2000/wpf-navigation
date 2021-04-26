using System.Collections.Generic;
using System.Collections.Immutable;

namespace acme.wpftdd.routes
{
    public class Routes
    {
        private readonly HashSet<UriRoute> _uriRoutes = new();
        private readonly HashSet<NamedRoute> _namedRoutes = new();

        // ReSharper disable once MemberCanBePrivate.Global
        public void Add(NamedRoute route) => _namedRoutes.Add(route);

        // ReSharper disable once MemberCanBePrivate.Global
        public void Add(UriRoute route) => _uriRoutes.Add(route);

        public void AddAll(params Route[] routes)
        {
            foreach (Route route in routes)
            {
                switch (route)
                {
                    case UriRoute uriRoute:
                        Add(uriRoute);
                        break;
                    case NamedRoute namedRoute:
                        Add(namedRoute);
                        break;
                }
            }
        }
        
        public IEnumerable<NamedRoute> NamedRoutes => _namedRoutes.ToImmutableList();
    }
}