using MongoDB.Bson;

namespace ArchitectNow.DataModels
{
    public class WodSection
    {
        public string Prefix { get; set; }
        public string DisplayName { get; set; }
        public string ComponentId { get; set; }
        public string RepScheme { get; set; }
        public string Details { get; set; }
        public MeasureType MeasureType { get; set; }
        public BsonDocument Measure { get; set; }
    }
}