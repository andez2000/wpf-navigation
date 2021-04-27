using System;

namespace acme.wpftdd.routes
{
    /// <summary>
    /// Defines a named route to a particular type of view.
    /// </summary>
    public class NamedRoute
    {
        public NamedRoute(string name, Type viewType)
        {
            Name = new Name(name);
            ViewType = viewType;
        }

        /// <summary>
        /// Gets the route name.
        /// </summary>
        public Name Name { get; }

        /// <summary>
        /// Gets the type of view.
        /// </summary>
        public Type ViewType { get; }

        private bool Equals(NamedRoute other)
        {
            return Equals(Name, other.Name) && ViewType == other.ViewType;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((NamedRoute) obj);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Name, ViewType);
        }
    }
}