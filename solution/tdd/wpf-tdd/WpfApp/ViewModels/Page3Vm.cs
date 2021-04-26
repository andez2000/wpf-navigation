using System.ComponentModel;

namespace acme.wpftdd.WpfApp.ViewModels
{
    public class Page3Vm : INotifyPropertyChanged
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