namespace wpftdd.routes
{
    public record Name
    {
        private readonly string _name;

        public Name(string name)
        {
            this._name = name;
        }

        public virtual bool Equals(Name other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return _name == other._name;
        }

        public override int GetHashCode()
        {
            return (_name != null ? _name.GetHashCode() : 0);
        }

        public static Name Named(string name)
        {
            return new Name(name);
        }
    }
}