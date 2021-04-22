using System;
using System.Collections.Generic;
using System.Windows;
using acme.external;
using acme.external.Pages;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestPlatform.ObjectModel;
using Xunit;

namespace wpf_tdd
{
    public class UnitTest1
    {
        [UIFact]
        public void Can_create_page_and_set_data_context()
        {
            ContainerBuilder containerBuilder = new ();
            
            containerBuilder.RegisterType<Page2WithVm>();
            containerBuilder.RegisterType<Page2Vm>();
            
            IContainer container = containerBuilder.Build();
            IServiceProvider serviceProvider = new AutofacServiceProvider(container);

            ElementDataContextThing thing = new ElementDataContextThing(serviceProvider);
            thing.Register<Page2WithVm, Page2Vm>((view, dataContext) =>
            {
                view.DataContext = dataContext;
            });

            var page2WithVm = thing.Resolve<Page2WithVm>();


            Assert.NotNull(page2WithVm);
        }
        
        
    }

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
    }
}