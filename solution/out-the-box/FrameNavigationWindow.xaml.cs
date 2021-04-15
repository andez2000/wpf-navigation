using System;
using System.Windows;

namespace out_the_box
{
    public partial class FrameNavigationWindow : Window
    {
        public FrameNavigationWindow()
        {
            InitializeComponent();
        }

        private void PageNoVM_OnClick(object sender, RoutedEventArgs e)
        {
            NavigationFrame.Navigate(new Uri("/Pages/PageNoVM.xaml", UriKind.RelativeOrAbsolute));
        }

        private void PageSimpleVM_OnClick(object sender, RoutedEventArgs e)
        {
            NavigationFrame.Navigate(new Uri("/Pages/PageSimpleVM.xaml", UriKind.RelativeOrAbsolute));
        }
    }
}