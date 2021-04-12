using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Navigation;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace WpfNav
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App
    {
        public App()
        {
            host = new HostBuilder()
                .UseDefaultServiceProvider(x => x.ValidateOnBuild = true)
                .ConfigureServices((context, services) => { ConfigureServices(services); })
                .Build();

            Services = host.Services;
            
            AppDomain.CurrentDomain.UnhandledException += (sender, args) =>
            {
                Console.WriteLine(args);
            };
        }

        private void ConfigureServices(IServiceCollection services)
        {
            services.AddSingleton<MainWindow>();
            services.AddSingleton<Scenario2Window>();
            
            services.AddAppNavigator(() => host.Services.GetService<MainWindow>()?.NavigationHost);
        }

        protected override void OnStartup(StartupEventArgs e)
        {
            MainWindow window = host.Services.GetService<MainWindow>();
            Debug.Assert(window != null, nameof(window) + " != null");
            window.Show();
        }

        private void Application_Startup(object sender, StartupEventArgs e)
        {
            
        }
        
        // [STAThreadAttribute]
        // public static int Main(string[] args)
        // {
        //     App app = new App();
        //
        //     app.InitializeComponent();
        //     app.Run();
        //
        //     return 0;
        // }

        
        private IHost host;
        public static IServiceProvider Services;
    }
}

// protected override void OnNavigating(NavigatingCancelEventArgs e)
// {
//     var eNavigator = e.Navigator;
//
//     string s = "";
//     base.OnNavigating(e);
//     
//     
// }
//
// protected override void OnLoadCompleted(NavigationEventArgs e)
// {
//     base.OnLoadCompleted(e);
//     
// }
//
// protected override void OnNavigated(NavigationEventArgs e)
// {
//     base.OnNavigated(e);
// }
//
// protected override void OnNavigationProgress(NavigationProgressEventArgs e)
// {
//     base.OnNavigationProgress(e);
// }
