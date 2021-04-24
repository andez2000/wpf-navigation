using System;
using System.Windows.Navigation;

namespace wpftdd
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
    }

    public delegate NavigationService ProvideNavigationService();
}