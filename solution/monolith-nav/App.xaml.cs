using System;
using System.Diagnostics;
using System.Windows;
using acme.monolith.Services;
using acme.monolith.ViewModels;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace acme.monolith
{
    public partial class App : Application
    {
        public App()
        {
            _host = new HostBuilder()
                .UseDefaultServiceProvider(x => x.ValidateOnBuild = true)
                .ConfigureServices((context, services) => { ConfigureServices(services); })
                .Build();

            _services = _host.Services;
            
            AppDomain.CurrentDomain.UnhandledException += (sender, args) =>
            {
                Console.WriteLine(args);
            };
        }
        
        private readonly IHost _host;
        private static IServiceProvider _services;

        private void ConfigureServices(IServiceCollection services)
        {
            services.AddSingleton<MainWindow>();
            
            services.AddAppNavigator(() => _host.Services.GetService<MainWindow>()?.NavigationHost.NavigationService);
        }

        protected override void OnStartup(StartupEventArgs e)
        {
            MainWindow window = _host.Services.GetService<MainWindow>();
            Debug.Assert(window != null, nameof(window) + " != null");
            
            window.DataContext = new MainWindowVm(_host.Services.GetService<PageNavigationService>());
            window.Show();
        }
    }
}