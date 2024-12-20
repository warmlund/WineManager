using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using WineManager.EntityModels;

namespace WineManager.DataContext.Sqlite
{
    public static class WineManagerContextExtensions
    {

        /// <summary>
        /// Adds the WineManagerContext to IServiceCollection
        /// Will be used by the web api for registering the database context
        /// </summary>
        /// <param name="services"></param>
        /// <param name="relativePath"></param>
        /// <param name="databaseName"></param>
        /// <returns></returns>
        /// <exception cref="FileNotFoundException"></exception>
        public static IServiceCollection AddWineManagerContext(this IServiceCollection services, string relativePath = "..", string databaseName = "WineManager.db")
        {
            string path = Path.Combine(relativePath, databaseName);
            path = Path.GetFullPath(path);
            WineManagerContextLogger.WriteLine($"Databse path: {path}");

            if (!File.Exists(path))
            {
                throw new FileNotFoundException(message: $"{path} not found.", fileName: path);
            }

            services.AddDbContext<WineManagerContext>(options =>
            {
                options.UseSqlite($"Data Source={path}");

                options.LogTo(WineManagerContextLogger.WriteLine, new[] { Microsoft.EntityFrameworkCore.Diagnostics.RelationalEventId.CommandExecuting });
            },

            contextLifetime: ServiceLifetime.Transient,
            optionsLifetime: ServiceLifetime.Transient);
        
            return services;
        }
    }
}
