using MongoDB.Driver;

namespace ArchitectNow.Mongo
{
    public interface IMongoDbContext
    {
        IMongoClient Client { get; }
        IMongoDatabase Database { get; }
        
        string DatabaseName { get; }
        
        bool CollectionExists<TType>();
        
        IMongoCollection<TType> GetCollection<TType>(string collectionName);
    }
}