using MongoDB.Bson.Serialization.Attributes;

namespace Mohjak.ContactManagement.Entities
{
    public class CustomField
    {
        [BsonElement("type")]
        public string Type { get; set; }

        [BsonElement("name")]
        public string Name { get; set; }

        [BsonElement("value")]
        public string Value { get; set; }
    }
}
