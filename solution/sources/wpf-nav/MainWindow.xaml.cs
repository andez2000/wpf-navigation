using System.Windows;

namespace WpfNav
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            Navigation.Content = new Navigator((AppNavigator)App.Services.GetService(typeof(AppNavigator)));
        }
    }
}