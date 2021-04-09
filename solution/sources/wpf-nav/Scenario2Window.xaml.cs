using System;
using System.Windows;

namespace WpfNav
{
    public partial class Scenario2Window : Window
    {
        public Scenario2Window()
        {
            InitializeComponent();
        }

        private bool switched;
        private void ButtonBase_OnClick(object sender, RoutedEventArgs e)
        {
            if (switched)
            {
                Frame1.Source = new Uri("View1.xaml", UriKind.RelativeOrAbsolute);
                Frame2.Source = new Uri("View2.xaml", UriKind.RelativeOrAbsolute);
            }
            else
            {
                Frame1.Source = new Uri("View2.xaml", UriKind.RelativeOrAbsolute);
                Frame2.Source = new Uri("View1.xaml", UriKind.RelativeOrAbsolute);
            }

            switched = !switched;
        }
    }
}