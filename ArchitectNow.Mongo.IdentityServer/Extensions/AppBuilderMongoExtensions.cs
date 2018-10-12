using System;
using IdentityServer4.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using MongoDB.Bson.Serialization;
using ArchitectNow.Mongo.Identity.Repositories;

namespace ArchitectNow.Mongo.Identity.Extensions
{
    public static class AppBuilderMongoExtensions
    {
        private static string  _newRepositoryMsg = $"Mongo Repository created/populated! Please restart your website, so Mongo driver will be configured  to ignore Extra Elements.";

        /// <summary>
        /// Adds the support for MongoDb persistence for all identityserver stored
        /// </summary>
        /// <remarks><![CDATA[
        /// It implements and used mongodb collections for:
        /// - Clients
        /// - IdentityResources
        /// - ApiResource
        /// ]]></remarks>
        public static void UseMongoDbForIdentityServer(this IApplicationBuilder app)
        {
            app.UseAuthentication().UseIdentityServer();
            
            //Resolve Repository with ASP .NET Core DI help 
            var repository = app.ApplicationServices.GetService<IMongoIdentityRepository>();
            var config = app.ApplicationServices.GetService<IIdentityConfig<AppUser, AppRole>>();

            // --- Configure Classes to ignore Extra Elements (e.g. _Id) when deserializing ---
            app.ConfigureIgnoreExtraElements();

            var createdNewRepository = false;
            if (!repository.CollectionExists<Client>())
            {
                foreach (var client in config.GetClients())
                    repository.Add(client);
                createdNewRepository = true;
            }

            //  --IdentityResource
            if (!repository.CollectionExists<IdentityResource>())
            {
                foreach (var res in config.GetIdentityResources())
                    repository.Add(res);
                createdNewRepository = true;
            }


            //  --ApiResource
            if (!repository.CollectionExists<ApiResource>())
            {
                foreach (var api in config.GetApiResources())
                    repository.Add(api);
                createdNewRepository = true;
            }
            
            //  --Initial Roles users
            if (!repository.CollectionExists<AppRole>(AppIndentityConstants.Mongo.IdentityRoleCollectionName))
            {
                foreach (var api in config.GetRoles())
                    repository.Add(api, AppIndentityConstants.Mongo.IdentityRoleCollectionName);
                createdNewRepository = true;
            }
            
            // If it's a new Repository (database), need to restart the website to configure Mongo to ignore Extra Elements.
            if (createdNewRepository)
                throw new Exception(_newRepositoryMsg);
        }


        /// <summary>
        /// Configure Classes to ignore Extra Elements (e.g. _Id) when deserializing
        /// As we are using "IdentityServer4.Models" we cannot add something like "[BsonIgnore]"
        /// </summary>
        public static IApplicationBuilder ConfigureIgnoreExtraElements(this IApplicationBuilder app)
        {
            BsonClassMap.RegisterClassMap<Client>(cm =>
            {
                cm.AutoMap();
                cm.SetIgnoreExtraElements(true);
            });
            BsonClassMap.RegisterClassMap<IdentityResource>(cm =>
            {
                cm.AutoMap();
                cm.SetIgnoreExtraElements(true);
            });
            BsonClassMap.RegisterClassMap<ApiResource>(cm =>
            {
                cm.AutoMap();
                cm.SetIgnoreExtraElements(true);
            });
            BsonClassMap.RegisterClassMap<PersistedGrant>(cm =>
            {
                cm.AutoMap();
                cm.SetIgnoreExtraElements(true);
            });
            
            return app;
        }
    }
}