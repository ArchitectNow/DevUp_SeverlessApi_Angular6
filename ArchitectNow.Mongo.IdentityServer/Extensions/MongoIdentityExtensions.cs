using System;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using ArchitectNow.Mongo.Identity.Repositories;
using ArchitectNow.Core.Mongo;

namespace ArchitectNow.Mongo.Identity.Extensions
{
    public static class MongoIdentityExtensions
    {
        
        public static IServiceCollection AddMongoIdentityRepository<TContext>(this IServiceCollection services,
            ServiceLifetime contextLifetime = ServiceLifetime.Transient) where TContext : IMongoIdentityRepository
        {
            services.TryAdd(new ServiceDescriptor(typeof(IMongoIdentityRepository), typeof(TContext), contextLifetime));
            return services;
        }

        public static IdentityBuilder AddMongoStores<TRepository>(this IdentityBuilder builder)
            where TRepository : IMongoIdentityRepository
        {
            var provider = builder.Services.BuildServiceProvider();
            var repo = (TRepository)provider.GetService<IMongoIdentityRepository>();
            
            builder.Services.AddSingleton<IUserStore<AppUser>>(p =>
            {
                IndexChecks.EnsureUniqueIndexOnNormalizedEmail(
                    repo.GetDatabase().GetCollection<AppUser>(AppIndentityConstants.Mongo.IdentityUserCollectionName));
                IndexChecks.EnsureUniqueIndexOnNormalizedUserName(
                    repo.GetDatabase().GetCollection<AppUser>(AppIndentityConstants.Mongo.IdentityUserCollectionName));
                return new AppUserStore<AppUser>(repo);
            });
            builder.Services.AddSingleton<IRoleStore<AppRole>>(p =>
            {
                IndexChecks.EnsureUniqueIndexOnNormalizedRoleName(
                    repo.GetDatabase().GetCollection<AppRole>(AppIndentityConstants.Mongo.IdentityRoleCollectionName));
                return new AppRoleStore<AppRole>(repo);
            });
            return builder;
        }
    }
}