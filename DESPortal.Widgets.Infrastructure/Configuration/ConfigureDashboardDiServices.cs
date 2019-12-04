using DESPortal.Widgets.Core.Services;
using DESPortal.Widgets.Infrastructure.Services;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace DESPortal.Widgets.Infrastructure.Configuration
{
    public static class ConfigureDashboardDiServices
    {
        public static IServiceCollection AddDashboardDiService(this IServiceCollection services)
        {
            services.AddScoped<IDashboardService, DashboardService>();
            return services;
        }
    }
}
