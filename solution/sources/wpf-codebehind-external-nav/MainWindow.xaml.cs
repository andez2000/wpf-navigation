using System.Windows;

namespace acme.wpfapp
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void FrameNavigation_OnClick(object sender, RoutedEventArgs e)
        {
            FrameNavigationWindow window = new FrameNavigationWindow();
            window.Show();
        }
    }
}