namespace ArchitectNow.Core.Mongo
{
    public class MongoConnectionOptions
    {
        public MongoConnection MongoConnection { get; set; }
    }

    public class MongoConnection
    {
        public string ConnectionString { get; set; }
        public string DatabaseName { get; set; }
    }
}