using System.Windows.Input;
using acme.foonav.Commands;
using acme.foonav.Services;
using wpf_nav_context;
using wpf_nav_context.ViewModels;

namespace acme.foonav.ViewModels
{
    public class Parent1 : BaseViewModel
    {
        public Parent1(ContextNavigationService navigationService)
        {
            Message = "Hello from Parent 1";
            Toggle = new RelayCommand(() =>
            {
                navigationService.NavigateTo("Parent2");
            }, () => true);
        }

        private string _message;

        public string Message
        {
            get => _message;
            set
            {
                _message = value;
                SetProperty(ref _message, value);
            }
        }

        public ICommand Toggle { get; }
    }
}