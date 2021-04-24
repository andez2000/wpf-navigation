﻿using System.ComponentModel;
using Microsoft.Extensions.DependencyInjection;
using wpftdd.views;

namespace wpftdd.routes
{
    public class ContainerRegistration
    {
        public void RegisterAll(Views views, IServiceCollection serviceCollection)
        {
            foreach (var view in views.AllViews())
            {
                serviceCollection.AddTransient(view);
            }
        }
    }
}