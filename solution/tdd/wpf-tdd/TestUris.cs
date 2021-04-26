using System;
using Xunit;

namespace acme.wpftdd
{
    public class TestUris
    {
        [UIFact]
        public void testem()
        {
            var uri1 = new Uri("/wpf-tdd;component/WpfApp/Pages/Page1.xaml", UriKind.RelativeOrAbsolute);
            var uri2 = new Uri("/wpf-tdd;component/WpfApp/Pages/Page1.xaml", UriKind.RelativeOrAbsolute);
            
            Assert.Equal(uri1, uri2);
        }
    }
}