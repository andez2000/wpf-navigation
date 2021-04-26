using System;
using System.Collections.Generic;
using System.Windows;

namespace acme.wpftdd
{
    public class ElementDataContextThing
    {
        private readonly Dictionary<Type, (Type, Delegate)> _viewTypeResolver = new ();
        private readonly IServiceProvider _serviceProvider;

        public ElementDataContextThing(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider ?? throw new ArgumentNullException(nameof(serviceProvider));
        }


        public void Register<TFrameworkElement, TDataContext>(Action<TFrameworkElement, TDataContext> action) 
            where TFrameworkElement : FrameworkElement 
            where TDataContext : new()
        {
            _viewTypeResolver.Add(typeof(TFrameworkElement), (typeof(TDataContext), action));
        }

        public TFrameworkElement Resolve<TFrameworkElement>()
        {
            var (dataContext, action) = _viewTypeResolver[typeof(TFrameworkElement)];

            TFrameworkElement frameworkElement = (TFrameworkElement) _serviceProvider.GetService(typeof(TFrameworkElement));
            object dataContextInstance =  _serviceProvider.GetService(dataContext);

            action.DynamicInvoke(frameworkElement, dataContextInstance);
            
            return frameworkElement;
        }

        

        //delegate void DataContextSetter(FrameworkElement frameworkElement, object viewModel);
    }
}