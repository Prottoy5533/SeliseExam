namespace EmployeeManage
{
    public static class ConfigureApiServices
    {
        public static IServiceCollection AddCustomAuthentication(this IServiceCollection services, IConfiguration configuration, bool isDevelopment)
        {
            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = "Cookies"; // Set the default scheme for authentication
                options.DefaultSignInScheme = "Cookies"; // Set the sign-in scheme to Cookies
            })
            .AddCookie("Cookies") // Add cookie authentication
            .AddJwtBearer("Bearer", options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ClockSkew = TimeSpan.Zero,
                    ValidIssuer = ConfigurationHelper.GetConfigValue(configuration, "Jwt:Issuer", isDevelopment),
                    ValidAudience = ConfigurationHelper.GetConfigValue(configuration, "Jwt:Issuer", isDevelopment),
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(ConfigurationHelper.GetConfigValue(configuration, "Jwt:Key", isDevelopment)))
                };
            });

            return services;
        }
    }
}
