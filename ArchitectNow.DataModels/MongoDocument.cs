using System;
using System.Collections.Generic;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson.Serialization.IdGenerators;

namespace ArchitectNow.DataModels
{
    public abstract class MongoDocument : IMongoDocument
    {
        [BsonRepresentation(BsonType.ObjectId)]
        [BsonId(IdGenerator = typeof (BsonObjectIdGenerator))]
        public ObjectId Id { get; set; }
        
        public DateTimeOffset CreatedDate { get; set; }
        public DateTimeOffset ModifiedDate { get; set; }
        
        [BsonExtraElements]
        public Dictionary<string, object> ExtraElements { get; set; }
    }
}