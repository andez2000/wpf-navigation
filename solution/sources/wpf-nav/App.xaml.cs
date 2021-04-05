using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace WpfNav
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public App()
        {
            host = new HostBuilder()
                .UseDefaultServiceProvider(x => x.ValidateOnBuild = true)
                .ConfigureServices((context, services) => { ConfigureServices(services); })
                .Build();
        }

        private void ConfigureServices(IServiceCollection services)
        {
            services.AddAppNavigator(() => host.Services.GetService<MainWindow>()?.NavigationHost);
        }

        private IHost host;
    }
}