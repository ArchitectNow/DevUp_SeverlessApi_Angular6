using System;
using System.Collections.Generic;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson.Serialization.IdGenerators;

namespace ArchitectNow.DataModels
{
    public class Wod : IMongoDocument
    {
        [BsonRepresentation(BsonType.ObjectId)]
        [BsonId(IdGenerator = typeof (BsonObjectIdGenerator))]
        public ObjectId Id { get; set; }
        public string LocationId { get; set; }
        public string ProgramId { get; set; }
        public string Program { get; set; }
        public string Name { get; set; }
        [BsonRepresentation(BsonType.String)]
        public DateTimeOffset WodDate { get; set; }
        [BsonRepresentation(BsonType.String)]
        public DateTimeOffset PublishOnDateTime { get; set; }
        public List<WodSection> Sections { get; set; }
        public string Notes { get; set; } 
    }
}