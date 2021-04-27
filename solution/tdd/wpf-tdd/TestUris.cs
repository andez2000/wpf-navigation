using System;
using Xunit;

namespace acme.wpftdd
{
    // https://docs.microsoft.com/en-us/dotnet/desktop/wpf/app-development/pack-uris-in-wpf?view=netframeworkdesktop-4.8
    public class TestUris
    {
        [UIFact]
        public void testem()
        {
            var uri1 = new Uri("wpf-tdd;component/WpfApp/Pages/Page1.xaml", UriKind.RelativeOrAbsolute);
            var uri2 = new Uri("wpf-tdd;component/WpfApp/Pages/Page1.xaml", UriKind.RelativeOrAbsolute);
            
            Assert.Equal(uri1, uri2);
        }
    }
}