using System;
using System.Collections.Generic;

namespace acme.foonav.Services
{
    public class ContextViewManager
    {
        //private readonly Dictionary<ViewContext, (ContextNavigationService, Action<object>)> _viewContexts = new ();
        private readonly Dictionary<Guid, Action<object>> _navigationServices = new ();
        public readonly object NavigatingLock = new object();
        
        public void Register(ViewContext context, ContextNavigationService navigationService, Action<object> dataContextChanger)
        {
           // _viewContexts.Add(context, (navigationService, dataContextChanger));
            
            lock (NavigatingLock)
            {
                _navigationServices.Add(navigationService.Id, dataContextChanger);
            }
        }

        public ContextNavigationService CurrentScope { get; private set; }

        public Action<object> StartNavigate(ContextNavigationService contextNavigationService)
        {
            lock (NavigatingLock)
            {
                CurrentScope = contextNavigationService;
                return _navigationServices[contextNavigationService.Id];
            }
        }
    }
}