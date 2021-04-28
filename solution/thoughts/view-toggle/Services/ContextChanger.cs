namespace wpf_nav_context.Services
{
    public class ContextChanger
    {
        private readonly ChangeContext _changeContext;
        public ProvideContextSource ContextSource { get; }

        public ContextChanger(ChangeContext changeContext, ProvideContextSource provideContextSource)
        {
            _changeContext = changeContext;
            ContextSource = provideContextSource;
        }
        
        public void Change(object newContext)
        {
            _changeContext(newContext);
        }
    }
}