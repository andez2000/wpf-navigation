using System;
using System.Collections.Generic;
using System.Windows;

namespace acme.wpftdd.views
{
    /// <summary>
    /// Provides the mapping for views and their data contexts (view models) with a delegate to callback to that can
    /// automatically set the data context on the keyed framework element or alternatively provide a custom callback
    /// which can control the setting up of the framework element's data context.
    /// </summary>
    public class Views
    {
        private readonly Dictionary<Type, (Type, Delegate)> _viewTypeResolver = new ();

        /// <summary>
        /// Registers the framework element and data context (view model) with a callback to set the data context on
        /// the framework element.
        /// </summary>
        /// <param name="setDataContext">The callback action to invoke when a framework element is resolved.</param>
        /// <typeparam name="TFrameworkElement">The type of framework element.</typeparam>
        /// <typeparam name="TDataContext">The type of data context.</typeparam>
        public void Register<TFrameworkElement, TDataContext>(Action<TFrameworkElement, TDataContext> setDataContext) 
            where TFrameworkElement : FrameworkElement 
            where TDataContext : new()
        {
            RegisterWithAction(setDataContext);
        }

        /// <summary>
        /// Registers the framework element and data context (view model) with an automatic setting of the framework
        /// element's DataContext property.
        /// </summary>
        /// <typeparam name="TFrameworkElement">The type of framework element.</typeparam>
        /// <typeparam name="TDataContext">The type of data context.</typeparam>
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

        /// <summary>
        /// Gets all registered views.
        /// </summary>
        /// <returns></returns>
        internal IEnumerable<Type> AllViews() => _viewTypeResolver.Keys;

        /// <summary>
        /// Gets the type of data context and the delegate to invoke for the type of view.
        /// </summary>
        /// <param name="viewType">The type of view to resolve.</param>
        /// <returns>A named tuple instance containing the type of data context and the delegate.</returns>
        public (Type dataContextType, Delegate action) ResolverFor(Type viewType)
        {
            // TODO: We are passing Delegate back but ultimately we need an
            //       Action<TFrameworkElement, TDataContext> setDataContext
            return _viewTypeResolver[viewType];
        }
    }
}