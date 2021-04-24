using System;
using System.Collections.Generic;
using System.Windows;

namespace wpftdd.views
{
    public class Views
    {
        private readonly Dictionary<Type, (Type, Delegate)> _viewTypeResolver = new ();

        public void Register<TFrameworkElement, TDataContext>(Action<TFrameworkElement, TDataContext> action) 
            where TFrameworkElement : FrameworkElement 
            where TDataContext : new()
        {
            RegisterWithAction<TFrameworkElement, TDataContext>(action);
        }

        public void Register<TFrameworkElement, TDataContext>() 
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
    }
}