using System;
using System.Windows.Navigation;

namespace acme.wpftdd
{
    public class NavigationController
    {
        private readonly ProvideNavigationService _provideNavigationService;

        public NavigationController(ProvideNavigationService provideNavigationService) =>
            _provideNavigationService =
                provideNavigationService ??
                throw new ArgumentNullException(nameof(provideNavigationService));

        public void NavigateTo(object content)
        {
            _provideNavigationService().Navigate(content);
        }
        
        public void NavigateTo(Uri uri)
        {
            var navigationService = _provideNavigationService();
            
            navigationService.Navigate(uri);
        }
    }

    public delegate NavigationService ProvideNavigationService();
}