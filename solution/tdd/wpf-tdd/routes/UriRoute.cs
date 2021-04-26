using System;

namespace acme.wpftdd.routes
{
    public sealed class UriRoute : Route
    {
        private readonly Uri _uri;

        public UriRoute(Uri uri)
        {
            _uri = uri;
        }

        private bool Equals(UriRoute other)
        {
            return Equals(_uri, other._uri);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((UriRoute) obj);
        }

        public override int GetHashCode()
        {
            return (_uri != null ? _uri.GetHashCode() : 0);
        }
    }
}