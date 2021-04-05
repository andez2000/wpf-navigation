using System;
using System.Windows.Controls;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace WpfNav
{
    public class AppNavigator
    {
        // I need...
        //  routes
        //  frame

        public Frame Frame
        {
            get => provideFrame();
        }

        private readonly ProvideFrame provideFrame;

        public AppNavigator(ProvideFrame provideFrame)
        {
            this.provideFrame = provideFrame;
        }

        public void NavigateTo(Uri uri)
        {
            Frame.Source = uri;
        }
    }
    
    public delegate Frame ProvideFrame();

    public static class AppNavigatorExtensions
    {
        // public static IHostBuilder AddAppNavigator(this IHostBuilder hostBuilder)
        // {
        //     return hostBuilder;
        // }
        
        public static IServiceCollection AddAppNavigator(this IServiceCollection serviceCollection, ProvideFrame provideFrame)
        {
            serviceCollection.AddSingleton(new AppNavigator(provideFrame));
            
            return serviceCollection;
        }
        
        
    }
}