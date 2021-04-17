using System;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Extensions.DependencyInjection;

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
            //frame.Source = uri;
            
            // works if you pass in the calling page, but only navigates on that one!
            // var navigationService = NavigationService.GetNavigationService((caller as DependencyObject)!);
            //navigationService.Navigate(uri);
            
            //frame.NavigationService.Navigate(uri);
            //frame.NavigationService.Refresh();
            frame.NavigationService.Navigate(uri);
            var navigationService = NavigationService.GetNavigationService(frame);
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