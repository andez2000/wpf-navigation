using System;

namespace acme.foonav.Services
{
    /// <summary>
    /// Simple View Resolver to invoke DI Container.
    /// </summary>
    public class ViewResolver
    {
        private readonly ProvideServiceFor _provideServiceFor;

        public ViewResolver(ProvideServiceFor provideService)
        {
            _provideServiceFor = provideService ?? throw new ArgumentNullException(nameof(provideService));
        }
        
        public object Resolve(Type viewType)
        {
            return _provideServiceFor(viewType);
        }
    }
    
    public delegate object ProvideServiceFor(Type serviceType);

}