using DESPortal.Widgets.Core.Services;
using DESPortal.Widgets.Infrastructure.Services;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace DESPortal.Widgets.Infrastructure.Configuration
{
    public static class ConfigureWidgetDiServices
    {
        public static IServiceCollection AddWidgetDiService(this IServiceCollection services)
        {
            services.AddScoped<IWidgetService, WidgetService>();
            return services;
        }
    }
}
