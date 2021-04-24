// using System;
// using System.Collections.Generic;
// using acme.external.Pages;
// using wpftdd.routes;
// using Xunit;
//
// namespace wpf_tdd
// {
//     public class RoutesTdd
//     {
//         [UIFact]
//         public void Test1()
//         {
//             Routes routes = new ();
//             routes.Register("some name", typeof(Page1));
//
//             Type routeType = routes.Resolve("some name");
//
//             ViewProvider provider = new ();
//             provider.Register(typeof(Page1), () => new Page1());
//             Page1 page1a = provider.For<Page1>();
//             Page1 page1b = (Page1)provider.For(routeType);
//             
//             Assert.NotNull(page1a);
//             Assert.NotNull(page1b);
//             
//             // register route to provide a type or an instance?
//             // if type then use a level of indirection around some ioc
//             // pass the view instance to the navigation service to navigate to
//         }
//     }
//
//     public class ViewProvider
//     {
//         private readonly Dictionary<Type, Func<object>> _creators = new();
//         
//         public TView For<TView>()
//         {
//             return (TView)_creators[typeof(TView)]();
//         }
//         
//         public object For(Type viewType)
//         {
//             return _creators[viewType]();
//         }
//
//         public void Register(Type viewType, Func<object> creator)
//         {
//             _creators.Add(viewType, creator);
//         }
//     }
// }