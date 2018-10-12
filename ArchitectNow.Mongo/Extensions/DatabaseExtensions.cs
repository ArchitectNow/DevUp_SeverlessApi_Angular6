using Microsoft.Extensions.DependencyInjection;
using ArchitectNow.Mongo.Repositories;

namespace ArchitectNow.Mongo.Extensions
{
    public static class DatabaseExtensions
    {
        public static IServiceCollection ConfigureRepositories(this IServiceCollection services)
        {
            services.AddSingleton<IMongoDbContext, MongoDbContext>();
            services.AddScoped<IWodsRepository, WodsRepository>();
            return services;
        }
    }
}