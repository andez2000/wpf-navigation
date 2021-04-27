using System;

namespace acme.wpftdd.views
{
    /// <summary>
    /// Responsible for resolving a view instance with the help from a service provider.
    /// </summary>
    public class ViewResolver
    {
        private readonly Views _views;
        private readonly ProvideServiceFor _provideServiceFor;

        public ViewResolver(Views views, ProvideServiceFor provideService)
        {
            _views = views ?? throw new ArgumentNullException(nameof(views));
            _provideServiceFor = provideService ?? throw new ArgumentNullException(nameof(provideService));
        }

        /// <summary>
        /// Resolves the type of view to an instance of that view. 
        /// </summary>
        /// <param name="viewType">The type of view to resolve.</param>
        /// <returns>An instance of the view.</returns>
        public object Resolve(Type viewType)
        {
            var (dataContextType, action) = _views.ResolverFor(viewType);

            var view = _provideServiceFor(viewType);
            var dataContext = _provideServiceFor(dataContextType);

            action.DynamicInvoke(view, dataContext);

            return view;
        }
    }

    /// <summary>
    /// Provides an instance given of service provided a particular service type.
    /// </summary>
    public delegate object ProvideServiceFor(Type serviceType);
}