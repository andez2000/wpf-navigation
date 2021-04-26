using System;
using System.Threading;
using acme.wpftdd.WpfApp.Pages;
using acme.wpftdd.WpfApp.Windows;
using Xunit;

namespace acme.wpftdd
{
    /// <summary>
    /// Tests around WPF and setting content with assertions to test out dispatching in tests only.
    /// </summary>
    public sealed class DirectySetFrameContentTests
    {
        [WpfFact]
        public void JustSetFrameContent2()
        {
            var runTestMonitor = new ManualResetEventSlim(false);
            var assertionsMonitor = new ManualResetEventSlim(false);
            var windowClosedMonitor = new ManualResetEventSlim(false);

            var (thread, mainWindow) = WindowDispatch.CreateWindowOnSTAThread(() => new MainWindow(), w => { });

            WindowDispatch.DispatchOn(mainWindow, () =>
            {
                var page1 = new Page1();
                mainWindow.NavigationHost.Content = page1;
            }, runTestMonitor, TimeSpan.FromSeconds(1));

            WindowDispatch.DispatchOn(mainWindow, () => Assert.NotNull(mainWindow.NavigationHost.Content),
                assertionsMonitor, TimeSpan.FromSeconds(1));
            WindowDispatch.DispatchOn(mainWindow, () => mainWindow.Close(), windowClosedMonitor,
                TimeSpan.FromSeconds(1));
        }
    }
}