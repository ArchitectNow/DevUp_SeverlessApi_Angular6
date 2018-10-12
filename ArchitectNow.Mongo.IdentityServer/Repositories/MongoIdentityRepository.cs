using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using MongoDB.Bson;
using MongoDB.Driver;
using ArchitectNow.Core.Mongo;

namespace ArchitectNow.Mongo.Identity.Repositories
{
    public interface IMongoIdentityRepository
    {
        IMongoDatabase GetDatabase();
        IQueryable<T> All<T>() where T : class, new();
        Task<List<T>> AllAsync<T>() where T : class, new();
        IQueryable<T> Where<T>(Expression<Func<T, bool>> expression) where T : class, new();
        void Delete<T>(Expression<Func<T, bool>> predicate) where T : class, new();
        T Single<T>(Expression<Func<T, bool>> expression) where T : class, new();
        bool CollectionExists<T>(string name = null) where T : class, new();
        void Add<T>(T item, string name = null) where T : class, new();
        Task AddAsync<T>(T item, string name = null) where T : class, new();
        void Add<T>(IEnumerable<T> items, string name = null) where T : class, new();
        Task AddAsync<T>(IEnumerable<T> items, string name = null) where T : class, new();
        Task<T> SingleAsync<T>(Expression<Func<T, bool>> expression);
        Task<List<T>> FindAsync<T>(Expression<Func<T, bool>> func);
    }

    public class MongoIdentityRepository : IMongoIdentityRepository
    {
        protected static IMongoClient Client;
        protected static IMongoDatabase Database;
        
        public MongoIdentityRepository(IOptions<MongoConnectionOptions> optionsAccessor)
        {
            var configurationOptions = optionsAccessor.Value;
            Client = new MongoClient(configurationOptions.MongoConnection.ConnectionString);
            Database = Client.GetDatabase(configurationOptions.MongoConnection.DatabaseName);    
        }
        
        /// <summary>
        /// Get Database connection
        /// </summary>
        /// <returns></returns>
        public IMongoDatabase GetDatabase()
        {
            return Database;
        }

        public IQueryable<T> All<T>() where T : class, new()
        {
            return Database.GetCollection<T>(typeof(T).Name).AsQueryable();
        }

        public Task<List<T>> AllAsync<T>() where T : class, new()
        {
            var filter = Builders<T>.Filter.Empty;
            return Database.GetCollection<T>(typeof(T).Name).Find(filter).ToListAsync();
        }

        public IQueryable<T> Where<T>(Expression<Func<T, bool>> expression) where T : class, new()
        {
            return All<T>().Where(expression);
        }

        public void Delete<T>(Expression<Func<T, bool>> predicate) where T : class, new()
        {
            Database.GetCollection<T>(typeof(T).Name).DeleteMany(predicate);
        }

        public T Single<T>(Expression<Func<T, bool>> expression) where T : class, new()
        {
            return All<T>().Where(expression).SingleOrDefault();
        }

        public bool CollectionExists<T>(string name = null) where T : class, new()
        {
            var collection = Database.GetCollection<T>(name ?? typeof(T).Name);
            var filter = new BsonDocument();
            var totalCount = collection.Count(filter);
            return totalCount > 0;
        }

        public void Add<T>(T item, string name = null) where T : class, new()
        {
            Database.GetCollection<T>(name ?? typeof(T).Name).InsertOne(item);
        }

        public Task AddAsync<T>(T item, string name = null) where T : class, new()
        {
            return Database.GetCollection<T>(name ?? typeof(T).Name).InsertOneAsync(item);
        }

        public void Add<T>(IEnumerable<T> items, string name = null) where T : class, new()
        {
            Database.GetCollection<T>(name ?? typeof(T).Name).InsertMany(items);
        }

        public Task AddAsync<T>(IEnumerable<T> items, string name = null) where T : class, new()
        {
            return Database.GetCollection<T>(name ?? typeof(T).Name).InsertManyAsync(items);
        }

        public Task<T> SingleAsync<T>(Expression<Func<T, bool>> expression)
        {
            var filter = Builders<T>.Filter.Where(expression);
            return Database.GetCollection<T>(typeof(T).Name).Find(filter).SingleOrDefaultAsync();
        }

        public Task<List<T>> FindAsync<T>(Expression<Func<T, bool>> expression)
        {
            var filter = Builders<T>.Filter.Where(expression);
            return Database.GetCollection<T>(typeof(T).Name).Find(filter).ToListAsync();
        }
    }
}