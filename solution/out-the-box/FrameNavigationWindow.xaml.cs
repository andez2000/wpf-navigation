using System;
using System.Windows;
using System.Windows.Controls;
using out_the_box.Pages;

namespace out_the_box
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
    }
}