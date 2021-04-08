using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Baml2006;
using System.Windows.Navigation;
using System.Xaml;
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

            NavigationService navigationService = null;
        }

        protected override void OnNavigating(NavigatingCancelEventArgs e)
        {
            var eNavigator = e.Navigator;

            string s = "";
            base.OnNavigating(e);
            
            
        }

        protected override void OnLoadCompleted(NavigationEventArgs e)
        {
            base.OnLoadCompleted(e);
            
        }

        protected override void OnNavigated(NavigationEventArgs e)
        {
            base.OnNavigated(e);
        }

        protected override void OnNavigationProgress(NavigationProgressEventArgs e)
        {
            base.OnNavigationProgress(e);
        }

        private IHost host;
    }
}