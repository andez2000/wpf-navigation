using acme.external;
using acme.external.Pages;
using acme.external.ViewModels;
using wpftdd.routes;
using wpftdd.views;
using Xunit;

namespace wpftdd
{
    public class SetupModuleTdd
    {
        [Fact]
        public void TddIt()
        {
            Routes routes = new Routes();
            routes.Add(new NamedRoute("Page1", typeof(Page1)));
            routes.Add(new NamedRoute("Page2", typeof(Page2WithVm)));
            routes.Add(new NamedRoute("Page2", typeof(Page3WithVm)));
            
            Views views = new Views();
            views.Register<Page2WithVm, Page2Vm>((view, dataContext) => view.DataContext = dataContext);
            views.Register<Page3WithVm, Page3Vm>();

        }
    }
}