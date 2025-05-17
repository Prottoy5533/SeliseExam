using EmployeeManagement.Application.Contracts.Services;
using EmployeeManagement.Application.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmployeeManagement.Application
{
    public static class ConfigureApplicationServices
    {
        public static IServiceCollection AddApplication(this IServiceCollection services, IConfiguration configuration, bool isDevelopment)
        {
            //var serviceProvider = services.BuildServiceProvider();

            services.AddScoped<ICurrentUserService, CurrentUserService>();


            services.AddMediatR(options =>
            {
                options.RegisterServicesFromAssembly(typeof(ConfigureApplicationServices).Assembly);


            });

            return services;
        }
    }
}
