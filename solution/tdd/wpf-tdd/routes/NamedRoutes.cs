using System.Collections.Generic;
using System.Collections.Immutable;

namespace acme.wpftdd.routes
{
    /// <summary>
    /// Defines the collection of named routes.
    /// </summary>
    public sealed class NamedRoutes
    {
        private readonly HashSet<NamedRoute> _namedRoutes = new();

        // ReSharper disable once MemberCanBePrivate.Global
        
        /// <summary>
        /// Adds the route to the registrations.
        /// </summary>
        /// <param name="route">The route to add.</param>
        public void Add(NamedRoute route) => _namedRoutes.Add(route);

        /// <summary>
        /// Adds the routes to the registrations.
        /// </summary>
        /// <param name="route">A route to add.</param>
        /// <param name="routes">The other routes to add.</param>
        public void AddAll(NamedRoute route, params NamedRoute[] routes)
        {
            _namedRoutes.Add(route);
            
            foreach (NamedRoute r in routes)
            {
                Add(r);
            }
        }

        /// <summary>
        /// Gets all of the named routes.
        /// </summary>
        public IEnumerable<NamedRoute> All => _namedRoutes.ToImmutableList();
    }
}