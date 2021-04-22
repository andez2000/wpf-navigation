using System;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using Xunit;

namespace wpf_tdd
{
    public class UnitTest1
    {
        [Fact]
        public void Can_create_page_and_set_data_context()
        {
            ContainerBuilder containerBuilder = new ();
            
            //containerBuilder.RegisterInstance
            //containerBuilder.RegisterType
            
            
            IContainer container = containerBuilder.Build();
            IServiceProvider autofacServiceProvider = new AutofacServiceProvider(container);
            
            
            
        }
    }
}