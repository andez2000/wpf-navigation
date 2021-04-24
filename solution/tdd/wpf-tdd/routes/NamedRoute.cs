using System;

namespace wpftdd.routes
{
    public class NamedRoute
    {
        private readonly string _name;
        private readonly Type _viewType;

        public NamedRoute(string name, Type viewType)
        {
            _name = name;
            _viewType = viewType;
        }
    }
}