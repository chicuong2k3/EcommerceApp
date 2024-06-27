using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace EcommerceApp.DAL
{
    public static class MigrationExtensions
    {
        public static IHost MigrateDatabase(this IHost host)
        {
            using (var scope = host.Services.CreateScope())
            {
                using (var context = scope.ServiceProvider.GetRequiredService<AppDbContext>())
                {
                    try
                    {
                        context.Database.Migrate();
                    }
                    catch (Exception e)
                    {
                        
                    }
                }
            }

            return host;
        }
    }
}
