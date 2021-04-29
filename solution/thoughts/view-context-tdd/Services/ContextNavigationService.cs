using System;
using System.Collections.Generic;
using acme.foonav.ViewModels;

namespace acme.foonav.Services
{
    public class ContextNavigationService
    {
        private readonly ContextViewManager _contextViewManager;
        private readonly ViewResolver _viewResolver;
        private readonly Dictionary<string, Type> _views = new();
        
        public Guid Id { get; } = Guid.NewGuid();

        public ContextNavigationService(ContextViewManager contextViewManager, ViewResolver viewResolver)
        {
            _contextViewManager = contextViewManager ?? throw new ArgumentNullException(nameof(contextViewManager));
            _viewResolver = viewResolver ?? throw new ArgumentNullException(nameof(viewResolver));
            
            _views.Add("Parent1", typeof(Parent1));
            _views.Add("Parent2", typeof(Parent2));
        }

        public void NavigateTo(string name)
        {
            lock (_contextViewManager.NavigatingLock)
            {
                Action<object> doNavigate = _contextViewManager.StartNavigate(this);
                var dataContext = _viewResolver.Resolve(_views[name]);

                doNavigate(dataContext);
            }
        }
    }
}