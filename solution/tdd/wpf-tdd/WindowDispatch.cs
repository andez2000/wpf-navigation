using System;
using System.Threading;
using System.Windows;
using System.Windows.Threading;

namespace wpftdd
{
    public static class WindowDispatch
    {
        public static (Thread thread, TWindow window) CreateWindowOnSTAThread<TWindow>(Func<TWindow> createWindow,
            Action<TWindow> postAction)
            where TWindow : Window
        {
            TWindow window = null;
            var waitUntilShow = new ManualResetEventSlim(false);

            var t = new Thread(() =>
            {
                window = createWindow();
                window.Closed += (s, e) => window.Dispatcher.InvokeShutdown();

                postAction(window);

                waitUntilShow.Set();

                System.Windows.Threading.Dispatcher.Run();
            });

            t.SetApartmentState(ApartmentState.STA);
            t.Start();
            waitUntilShow.Wait(TimeSpan.FromSeconds(2));
            Thread.Sleep(100);

            return (t, window);
        }

        internal static void DispatchOn(Window window, Action action, ManualResetEventSlim manualResetEvent,
            TimeSpan timeout)
        {
            window.Dispatcher.BeginInvoke(() =>
            {
                action();
                manualResetEvent.Set();
            }, DispatcherPriority.Normal);

            manualResetEvent.Wait(timeout);
        }

        internal static TProperty GetProperty<TProperty>(Window window, Func<TProperty> getProperty)
        {
            ManualResetEventSlim manualResetEvent = new ManualResetEventSlim(false);
            TimeSpan timeout = TimeSpan.FromSeconds(1);

            TProperty property = default(TProperty);

            window.Dispatcher.BeginInvoke(() =>
            {
                property = getProperty();
                manualResetEvent.Set();
            }, DispatcherPriority.Normal);

            manualResetEvent.Wait(timeout);

            return property;
        }
    }
}