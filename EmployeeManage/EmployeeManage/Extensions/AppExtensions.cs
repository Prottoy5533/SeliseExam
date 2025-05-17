using Microsoft.EntityFrameworkCore;

namespace EmployeeManage.Extensions
{
    public static class AppExtensions
    {
        public static WebApplication MigrateDatabase<TContext>(this WebApplication app,
        Action<TContext> seeder, int retry = 0) where TContext : DbContext
        {
            using (var scope = app.Services.CreateScope())
            {
                var services = scope.ServiceProvider;
                var context = services.GetRequiredService<TContext>();

                try
                {
                    InvokeSeeder(seeder, context);
                }
                catch (Exception ex)
                {
                    if (retry <= 10) 
                    {
                        retry++;
                        System.Threading.Thread.Sleep(2000);
                        MigrateDatabase<TContext>(app, seeder, retry);
                    }
                }
            }
            return app;
        }

        private static void InvokeSeeder<TContext>(Action<TContext> seeder, TContext context) where TContext : DbContext
        {
            context.Database.Migrate(); 
            seeder(context);
        }
    }
}
