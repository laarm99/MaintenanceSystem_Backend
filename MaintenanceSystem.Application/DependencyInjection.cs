using Microsoft.Extensions.DependencyInjection;
using MaintenanceSystem.Application.Modules.Dashboard;
using System;
using System.Collections.Generic;
using System.Text;

namespace MaintenanceSystem.Application
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApplication(

       this IServiceCollection services)
        {
            services.AddScoped<DashboardService>();

            return services;
        }
    }
}
