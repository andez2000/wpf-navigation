using System;
using System.Windows;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace wpf_nav_context
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private readonly IHost _host;
        private static IServiceProvider _serviceProvider;
        
        public App()
        {
            _host = new HostBuilder()
                .UseDefaultServiceProvider(x => x.ValidateOnBuild = true)
                .ConfigureServices((context, services) => { ConfigureServices(services); })
                .Build();

            _serviceProvider = _host.Services;
            
            AppDomain.CurrentDomain.UnhandledException += (sender, args) =>
            {
                Console.WriteLine(args);
            };
        }
        
        private void ConfigureServices(IServiceCollection services)
        {
        }
        
        protected override void OnStartup(StartupEventArgs e)
        {
            MainWindow window = new MainWindow();
            window.Show();
        }
    }
}