using System.ComponentModel;

namespace acme.external.ViewModels
{
    public class Page2Vm : INotifyPropertyChanged
    {
        private string _message;
        public event PropertyChangedEventHandler PropertyChanged;

        public string Message
        {
            get => _message;
            set { _message = value; OnPropertyChanged(nameof(Message)); }
        }

        private void OnPropertyChanged(string propertyName) => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}