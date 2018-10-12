using IdentityServer4.Services;
using IdentityServer4.Stores;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace ArchitectNow.Identity.IdentityServer
{
    public static class IdentityServerExtensions
    {
        /// <summary>
        /// Configure ClientId / Secrets
        /// </summary>
        /// <param name="builder"></param>
        /// <param name="configurationOption"></param>
        /// <returns></returns>
        public static IIdentityServerBuilder AddClients(this IIdentityServerBuilder builder)
        {
            builder.Services.AddTransient<IClientStore, AmrapsClientStore>();
            builder.AddClientStore<AmrapsClientStore>();
            return builder;
        }


        /// <summary>
        /// Configure API  &  Resources
        /// Note: Api's have also to be configured for clients as part of allowed scope for a given clientID 
        /// </summary>
        /// <param name="builder"></param>
        /// <returns></returns>
        public static IIdentityServerBuilder AddIdentityApiResources(this IIdentityServerBuilder builder)
        {
            builder.Services.AddTransient<IResourceStore, AmrapsResourceStore>();
            return builder;
        }

        /// <summary>
        /// Configure Grants
        /// </summary>
        /// <param name="builder">The builder.</param>
        /// <returns></returns>
        public static IIdentityServerBuilder AddPersistedGrants(this IIdentityServerBuilder builder)
        {
            builder.Services.AddTransient<IPersistedGrantStore, AmrapsPersistedGrantStore>();
            return builder;
        }
        
        /// <summary>
        /// Configure Profile Service
        /// </summary>
        /// <param name="builder">The builder.</param>
        /// <returns></returns>
        public static IIdentityServerBuilder AddProfileService(this IIdentityServerBuilder builder)
        {
            builder.Services.AddTransient<IProfileService, AmrapsProfileService>();
            return builder;
        }
    }
}