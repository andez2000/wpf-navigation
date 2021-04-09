using System;
using System.Windows;
using System.Windows.Controls;

namespace WpfNav
{
    public partial class Navigator : Page
    {
        private readonly AppNavigator _appNavigator;

        public Navigator()
        {
            InitializeComponent();
        }

        public Navigator(AppNavigator appNavigator)
        {
            _appNavigator = appNavigator;
            
            InitializeComponent();
        }

        private void View1_OnClick(object sender, RoutedEventArgs e)
        {
            _appNavigator.NavigateTo(new Uri("pack://application:,,,/View1.xaml"));
        }
        
        private void View2_OnClick(object sender, RoutedEventArgs e)
        {
            _appNavigator.NavigateTo(new Uri("pack://application:,,,/View2.xaml"));
            Page x;
        }
    }
    
    //XpfXamlLoader;
}