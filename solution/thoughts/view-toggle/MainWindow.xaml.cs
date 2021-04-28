using System.Windows;
using wpf_nav_context.Services;
using wpf_nav_context.ViewModels;

namespace wpf_nav_context
{
    // if any service served is IDisposable then we need to be very wary if in scope!
    //parent1 ??= (ContextParent) scope.ServiceProvider.GetService(typeof(ContextParent));

    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            
            ViewModelPanelTop.DataContext = CreateParent1(() => ViewModelPanelTop);
            ViewModelPanelBottom.DataContext = CreateParent2(() => ViewModelPanelBottom);
        }

        private Parent1 CreateParent1(ProvideContextSource provideContextSource)
        {
            return new(
                new ContextChanger(
                    context => { provideContextSource().DataContext = context; },
                    provideContextSource
                ), CreateParent2
            );
        }
        
        private Parent2 CreateParent2(ProvideContextSource provideContextSource)
        {
            return new(
                new ContextChanger(
                    context => { provideContextSource().DataContext = context; },
                    provideContextSource
                ), CreateParent1
            );
        }
    }

    public delegate FrameworkElement ProvideContextSource();
    public delegate void ChangeContext(object newContext);
    
    public delegate Parent1 CreateParent1(ProvideContextSource provideContextSource);
    public delegate Parent2 CreateParent2(ProvideContextSource provideContextSource);
}