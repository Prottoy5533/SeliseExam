using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmployeeManagement.Application.Common.Helper
{
    public static class ConfigurationHelper
    {
        public static string GetConfigValue(IConfiguration configuration, string key, bool isDevelopment)
        {
            if (isDevelopment)
            {
                return configuration[key] ?? "";
            }

            //For environment varilable, key "k1:k2:k3" should be "k1__k2__k3"
            //EMNP is the prefix for app specific environment variables
            return Environment.GetEnvironmentVariable($"{configuration["AppPrefix"]}__{key.Replace(":", "__")}",
                EnvironmentVariableTarget.User) ?? "";
        }
    }
}
