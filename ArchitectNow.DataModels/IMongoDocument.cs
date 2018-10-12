using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson.Serialization.IdGenerators;

namespace ArchitectNow.DataModels
{
    public interface IMongoDocument
    {
        [BsonRepresentation(BsonType.ObjectId)]
        [BsonId(IdGenerator = typeof (BsonObjectIdGenerator))]
        ObjectId Id { get; set; }
    }
}