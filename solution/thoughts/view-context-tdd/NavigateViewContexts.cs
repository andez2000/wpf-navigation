using System;
using System.Threading;
using acme.foonav.Services;
using acme.foonav.ViewModels;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace acme.foonav
{
    public class NavigateViewContexts
    {
        private readonly IServiceProvider _serviceProvider;
        private (Thread Thread, MainWindow mainWindow) _context;
        private readonly ContextViewManager _contextViewManager;
        private readonly ViewResolver _viewResolver;
        private readonly ViewContext _contextTop = new("ContextTop");
        private readonly ViewContext _contextBottom = new("ContextBottom");

        public NavigateViewContexts()
        {
            _contextViewManager = new ContextViewManager();
            _viewResolver = new ViewResolver((view) => _serviceProvider.GetService(view));
            
            ServiceCollection serviceCollection = new();

            serviceCollection.AddTransient<Parent1>();
            serviceCollection.AddTransient<Parent2>();
            serviceCollection.AddSingleton(_contextViewManager);
            serviceCollection.AddSingleton(_viewResolver);
            serviceCollection.AddTransient((c) => 
                _contextViewManager.CurrentScope ?? 
                new ContextNavigationService(_contextViewManager, _viewResolver));
            
            ContainerBuilder containerBuilder = new();
            containerBuilder.Populate(serviceCollection);
            IContainer container = containerBuilder.Build();
            _serviceProvider = new AutofacServiceProvider(container);
        }

        [UIFact]
        public void Can_navigate_multiple_view_contexts()
        {
            _context = WindowDispatch.CreateWindowOnSTAThread(() => new MainWindow(), w => { });
            ContextNavigationService topNav = new ContextNavigationService(_contextViewManager, _viewResolver); 
            ContextNavigationService bottomNav = new ContextNavigationService(_contextViewManager, _viewResolver);
            
            _contextViewManager.Register(_contextTop, 
                topNav,
                o => _context.mainWindow.Dispatch((w) => w.ViewModelPanelTop.DataContext = o));
            
            _contextViewManager.Register(_contextBottom,
                bottomNav,
                o => _context.mainWindow.Dispatch((w) => w.ViewModelPanelBottom.DataContext = o));
            
            //BOO No good here!  We need to setup the view context navigation otherwise it just a new instance
            // we circumvent this calling StartNavigate.  
            // now we need to try and make this nice.  perhaps we need a register root view along with things
            _contextViewManager.StartNavigate(topNav);
            _context.mainWindow.Dispatch((w) => w.ViewModelPanelTop.DataContext = _serviceProvider.GetService(typeof(Parent1)));

            _contextViewManager.StartNavigate(bottomNav);
            _context.mainWindow.Dispatch((w) => w.ViewModelPanelBottom.DataContext = _serviceProvider.GetService(typeof(Parent2)));

            Assert.Equal(typeof(Parent1), _context.mainWindow.Get(w => w.ViewModelPanelTop.DataContext).GetType());
            Assert.Equal(typeof(Parent2), _context.mainWindow.Get(w => w.ViewModelPanelBottom.DataContext).GetType());

            // tell parent 1 to navigate to parent 2
            _context.mainWindow.Dispatch((w) => ((Parent1)w.ViewModelPanelTop.DataContext).Toggle.Execute(null));
            
            Assert.Equal(typeof(Parent2), _context.mainWindow.Get(w => w.ViewModelPanelTop.DataContext).GetType());
            
            // tell parent 2 to navigate to parent 1
            _context.mainWindow.Dispatch((w) => ((Parent2)w.ViewModelPanelTop.DataContext).Toggle.Execute(null));
            
            Assert.Equal(typeof(Parent1), _context.mainWindow.Get(w => w.ViewModelPanelTop.DataContext).GetType());
            
            
        }
    }
}