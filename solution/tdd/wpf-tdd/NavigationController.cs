using System;
using System.Windows.Navigation;

namespace acme.wpftdd
{
    /// <summary>
    /// Responsible for the front end navigation.
    /// </summary>
    public class NavigationController
    {
        private readonly ProvideNavigationService _provideNavigationService;

        public NavigationController(ProvideNavigationService provideNavigationService) =>
            _provideNavigationService =
                provideNavigationService ??
                throw new ArgumentNullException(nameof(provideNavigationService));

        /// <summary>
        /// Navigates to the specified content.
        /// </summary>
        /// <param name="content">The content to navigate to.</param>
        /// <remarks>
        /// <paramref name="content"/> can be anything that the WPF framework uses.  Therefore this could be:
        /// <list type="bullet">
        /// <item><description>Content such as an instance of a page</description></item>
        /// </list>
        /// </remarks>
        public void NavigateTo(object content)
        {
            _provideNavigationService().Navigate(content);
        }
        
        /// <summary>
        /// Navigates to the specified uri.
        /// </summary>
        /// <param name="uri">The uri to navigate to.</param>
        public void NavigateTo(Uri uri)
        {
            var navigationService = _provideNavigationService();
            
            navigationService.Navigate(uri);
        }
    }

    public delegate NavigationService ProvideNavigationService();
}