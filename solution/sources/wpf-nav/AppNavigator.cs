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

        private readonly ProvideFrame provideFrame;

        public AppNavigator(ProvideFrame provideFrame)
        {
            this.provideFrame = provideFrame;
        }

        public void NavigateTo(Uri uri)
        {
            var frame = provideFrame();
            //frame.Content = null;
            frame.Source = uri;
            
            //frame.NavigationService.LoadCompleted += (sender, args) => frame.NavigationService.Refresh(); 
        }
    }
    
    public delegate Frame ProvideFrame();

    public static class AppNavigatorExtensions
    {
        public static IServiceCollection AddAppNavigator(this IServiceCollection serviceCollection, ProvideFrame provideFrame)
        {
            serviceCollection.AddSingleton(new AppNavigator(provideFrame));
            
            return serviceCollection;
        }
        
        
    }
}