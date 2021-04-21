using System;
using System.Windows.Input;
using acme.monolith.Commands;
using acme.monolith.Services;

namespace acme.monolith.ViewModels
{
    public class MainWindowVm : BaseViewModel
    {
        private readonly PageNavigationService _pageNavigationService;
        
        public ICommand NavigateTo { get; }

        internal MainWindowVm(PageNavigationService pageNavigationService)
        {
            _pageNavigationService = pageNavigationService ?? throw new ArgumentNullException(nameof(pageNavigationService));
            
            NavigateTo = new RelayCommand<string>(NavigateToView, (_) => true);
        }

        private void NavigateToView(string view)
        {
            _pageNavigationService.NavigateTo(new Uri(view, UriKind.RelativeOrAbsolute));
        }
    }
}