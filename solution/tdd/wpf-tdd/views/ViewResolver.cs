using System;

namespace acme.wpftdd.views
{
    public class ViewResolver
    {
        private readonly Views _views;
        private readonly ProvideServiceFor _provideServiceFor;

        public ViewResolver(Views views, ProvideServiceFor provideService)
        {
            _views = views ?? throw new ArgumentNullException(nameof(views));
            _provideServiceFor = provideService ?? throw new ArgumentNullException(nameof(provideService));
        }

        public object Resolve(Type viewType)
        {
            var (dataContextType, action) = _views.ResolverFor(viewType);

            var view = _provideServiceFor(viewType);
            var dataContext = _provideServiceFor(dataContextType);

            action.DynamicInvoke(view, dataContext);

            return view;
        }
    }

    public delegate object ProvideServiceFor(Type serviceType);
}