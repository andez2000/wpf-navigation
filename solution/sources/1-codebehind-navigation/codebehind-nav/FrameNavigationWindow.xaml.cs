using System;
using System.Windows;
using acme.wpfapp.Pages;

namespace acme.wpfapp
{
    public partial class FrameNavigationWindow : Window
    {
        public FrameNavigationWindow()
        {
            InitializeComponent();
        }

        private void UriToPageNoVM_OnClick(object sender, RoutedEventArgs e)
        {
            NavigationFrame.Navigate(new Uri("/Pages/PageNoVM.xaml", UriKind.RelativeOrAbsolute));
        }

        private void UriToPageSimpleVM_OnClick(object sender, RoutedEventArgs e)
        {
            NavigationFrame.Navigate(new Uri("/Pages/PageSimpleVM.xaml", UriKind.RelativeOrAbsolute));
        }

        private void ContentPageNoVM_OnClick(object sender, RoutedEventArgs e)
        {
            NavigationFrame.Content = new PageNoVM();
        }

        private void ContentPageSimpleVM_OnClick(object sender, RoutedEventArgs e)
        {
            NavigationFrame.Content = new PageSimpleVM();
        }
    }
}