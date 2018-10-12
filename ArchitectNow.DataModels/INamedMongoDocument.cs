using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson.Serialization.IdGenerators;

namespace ArchitectNow.DataModels
{
    public interface INamedMongoDocument
    {
        [BsonRepresentation(BsonType.String)]
        [BsonId(IdGenerator = typeof (StringObjectIdGenerator))]
        string Id { get; set; }
        string Name { get; set; }
    }
}