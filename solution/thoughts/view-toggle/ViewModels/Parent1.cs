using System.Windows.Input;
using wpf_nav_context.Commands;
using wpf_nav_context.Services;

namespace wpf_nav_context.ViewModels
{
    public class Parent1 : BaseViewModel
    {
        public Parent1(ContextChanger contextChanger, CreateParent2 createParent2)
        {
            var contextChanger1 = contextChanger;
            Message = "Hello from Parent 1";
            Toggle = new RelayCommand(() =>
            {
                contextChanger1.Change(createParent2(contextChanger1.ContextSource));
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