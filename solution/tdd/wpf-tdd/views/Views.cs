using System;
using System.Collections.Generic;
using System.Windows;

namespace acme.wpftdd.views
{
    public class Views
    {
        private readonly Dictionary<Type, (Type, Delegate)> _viewTypeResolver = new ();

        public void Register<TFrameworkElement, TDataContext>(Action<TFrameworkElement, TDataContext> action) 
            where TFrameworkElement : FrameworkElement 
            where TDataContext : new()
        {
            RegisterWithAction(action);
        }

        public void RegisterForAutoDataContext<TFrameworkElement, TDataContext>() 
            where TFrameworkElement : FrameworkElement 
            where TDataContext : new()
        {
            RegisterWithAction<TFrameworkElement, TDataContext>((view, dataContext) => view.DataContext = dataContext);
        }
        
        private void RegisterWithAction<TFrameworkElement, TDataContext>(Action<TFrameworkElement, TDataContext> action)
        {
            _viewTypeResolver.Add(typeof(TFrameworkElement), (typeof(TDataContext), action));
        }

        internal IEnumerable<Type> AllViews() => _viewTypeResolver.Keys;

        public (Type dataContextType, Delegate action) ResolverFor(Type viewType)
        {
            return _viewTypeResolver[viewType];
        }
    }
}