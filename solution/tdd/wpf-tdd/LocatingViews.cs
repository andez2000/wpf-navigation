using System;
using acme.external;
using acme.external.Pages;
using acme.external.ViewModels;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestPlatform.ObjectModel;
using wpftdd;
using Xunit;

namespace wpf_tdd
{
    public class LocatingViews
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
            Assert.NotNull(page2WithVm.DataContext);
        }
        
        
    }
}