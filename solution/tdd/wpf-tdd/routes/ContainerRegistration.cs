using acme.wpftdd.views;
using Microsoft.Extensions.DependencyInjection;

namespace acme.wpftdd.routes
{
    public static class ContainerRegistration
    {
        public static void RegisterAll(Views views, IServiceCollection serviceCollection)
        {
            foreach (var view in views.AllViews())
            {
                serviceCollection.AddTransient(view);
            }
        }
    }
}