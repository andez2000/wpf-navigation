using System;
using System.Threading;
using System.Windows;
using System.Windows.Threading;

namespace acme.foonav
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

        internal static void DispatchOnWithWait(Window window, Action action)
        {
            var monitor = new ManualResetEventSlim(false);

            window.Dispatcher.BeginInvoke(() =>
            {
                action();
                monitor.Set();
            }, DispatcherPriority.Normal);

            monitor.Wait(TimeSpan.FromSeconds(1));
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

    public static class WindowExtensions
    {
        public static void Dispatch(this Window window, Action action)
        {
            var monitor = new ManualResetEventSlim(false);

            window.Dispatcher.BeginInvoke(() =>
            {
                action();
                monitor.Set();
            }, DispatcherPriority.Normal);

            monitor.Wait(TimeSpan.FromSeconds(1));
        }

        public static void Dispatch<TWindow>(this TWindow window, Action<TWindow> action) where TWindow : Window
        {
            var monitor = new ManualResetEventSlim(false);

            window.Dispatcher.BeginInvoke(() =>
            {
                action(window);
                monitor.Set();
            }, DispatcherPriority.Normal);

            monitor.Wait(TimeSpan.FromSeconds(1));
        }
        
        public static TProp Get<TWindow, TProp>(this TWindow window, Func<TWindow, TProp> func) where TWindow : Window
        {
            var monitor = new ManualResetEventSlim(false);

            TProp prop = default(TProp);

            window.Dispatcher.BeginInvoke(() =>
            {
                prop = func(window);
                monitor.Set();
            }, DispatcherPriority.Normal);

            monitor.Wait(TimeSpan.FromSeconds(1));

            return prop;
        }
    }
}