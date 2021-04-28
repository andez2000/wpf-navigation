using System.Windows.Input;
using wpf_nav_context.Commands;
using wpf_nav_context.Services;

namespace wpf_nav_context.ViewModels
{
    public class Parent2 : BaseViewModel
    {
        public Parent2(ContextChanger contextChanger, CreateParent1 createParent1)
        {
            var contextChanger1 = contextChanger;
            Message = "Hello from Parent 2";
            Toggle = new RelayCommand(() =>
            {
                contextChanger1.Change(createParent1(contextChanger1.ContextSource));
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