using System;
using System.Windows;
using System.Windows.Controls;

namespace acme.wpfapp
{
    public partial class FrameNavigationWindow : Window
    {
        public FrameNavigationWindow()
        {
            InitializeComponent();
        }

        private void ExternalUriToPageNoVM_OnClick(object sender, RoutedEventArgs e)
        {
            NavigationFrame.Navigate(new Uri(
                @"D:\vcs\github\andez2000\wpf-navigation\external\Pages\ExternalPageNoVM.xaml", UriKind.RelativeOrAbsolute));
        }

        private void ExternalUriToPageSimpleVM_OnClick(object sender, RoutedEventArgs e)
        {
            // this all doesnt work
            
            // var loadComponent = (Page)Application.LoadComponent(new Uri(
            //     @"D:\vcs\github\andez2000\wpf-navigation\external\Pages\ExternalPageSimpleVM.xaml",
            //     UriKind.RelativeOrAbsolute));

            // cannot create the vm in 

            NavigationFrame.Navigate(
                new Uri(
                @"D:\vcs\github\andez2000\wpf-navigation\external\Pages\ExternalPageSimpleVM.xaml", UriKind.RelativeOrAbsolute));
        }

        private void OtherAssemblyPageNoVM_OnClick(object sender, RoutedEventArgs e)
        {
            NavigationFrame.Navigate(new Uri(
                "pack://application:,,,/external-page-lib;component/Pages/Page1.xaml", UriKind.RelativeOrAbsolute));
        }
    }
}