namespace acme.foonav.Services
{
    public class ViewContext
    {
        private string key;

        public ViewContext(string key)
        {
            this.key = key;
        }

        private bool Equals(ViewContext other)
        {
            return key == other.key;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((ViewContext) obj);
        }

        public override int GetHashCode()
        {
            return (key != null ? key.GetHashCode() : 0);
        }
    }
}