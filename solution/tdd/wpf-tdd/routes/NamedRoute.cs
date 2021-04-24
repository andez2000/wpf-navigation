using System;

namespace wpftdd.routes
{
    public class NamedRoute : Route
    {
        private readonly Name _name;
        private readonly Type _viewType;

        public NamedRoute(string name, Type viewType)
        {
            _name = new Name(name);
            _viewType = viewType;
        }

        // ReSharper disable once ConvertToAutoProperty
        public Name Name => _name;

        // ReSharper disable once ConvertToAutoProperty
        public Type ViewType => _viewType;
    }
}