using System;
using System.Runtime.CompilerServices;
using ArchitectNow.Mongo;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Configuration;
using MongoDB.Driver;

namespace ArchitectNow.ApiFunctions.Shared
{
    public static class Application
    {
        private static IConfigurationRoot _config;
        public static IMongoDbContext DataContext;
        public static Repositories Repositories = new Repositories();
        
        public static ExecutionContext Initialize(this ExecutionContext context)
        {
            context.InitializeConfiguration().InitializeDatabaseClient("mongoConnection", "amraps-dev");
            return context;
        }
        
        public static ExecutionContext InitializeConfiguration(this ExecutionContext context)
        {
            if (_config != null)
                return context;
            
            _config = new ConfigurationBuilder()
                       .SetBasePath(context.FunctionAppDirectory)
                       .AddJsonFile("local.settings.json", optional: true, reloadOnChange: true)
                       .AddEnvironmentVariables()
                       .Build();
            return context;
        }

        public static ExecutionContext InitializeDatabaseClient(this ExecutionContext context, 
            string connectionStringName, string databaseName)
        {
            if (_config == null)
                context.InitializeConfiguration();
       
            var connectionString = _config.GetConnectionString(connectionStringName);
            DataContext = new MongoDbContext(connectionString, databaseName);
          
            return context;
        }

        public static TType GetConfigValue<TType>(string keyname)
        {
            if (_config == null)
                throw new Exception("Please initialize the configuration before trying to get value.");

            return _config.GetValue<TType>(keyname);
        }
    }
}