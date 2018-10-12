using System.Collections.Generic;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson.Serialization.IdGenerators;

namespace ArchitectNow.DataModels
{
    public abstract class NamedMongoDocument : INamedMongoDocument
    {
        [BsonRepresentation(BsonType.String)]
        [BsonId(IdGenerator = typeof (StringObjectIdGenerator))]
        public string Id { get; set; }
        public string Name { get; set; }
        
        [BsonExtraElements]
        public Dictionary<string, object> ExtraElements { get; set; }
    }
}