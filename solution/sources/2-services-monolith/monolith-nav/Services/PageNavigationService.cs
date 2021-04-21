using System;
using System.Windows.Navigation;
using Microsoft.Extensions.DependencyInjection;

namespace acme.monolith.Services
{
    public class PageNavigationService
    {
        private readonly ProvideNavigationService _provideService;

        public PageNavigationService(ProvideNavigationService provideService)
        {
            this._provideService = provideService;
        }

        public void NavigateTo(Uri uri)
        {
            var service = _provideService();
            service.Navigate(uri);
        }
    }
    
    public delegate NavigationService ProvideNavigationService();
    
    public static class AppNavigatorExtensions
    {
        public static IServiceCollection AddAppNavigator(this IServiceCollection serviceCollection, ProvideNavigationService provideFrame)
        {
            serviceCollection.AddSingleton(new PageNavigationService(provideFrame));
            
            return serviceCollection;
        }
    }
}