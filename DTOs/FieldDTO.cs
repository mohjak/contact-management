using MongoDB.Bson.Serialization.Attributes;

namespace Mohjak.ContactManagement.DTOs
{
    public class FieldDTO
    {
        [BsonElement("isExisting")]
        public bool IsExisting { get; set; }

        [BsonElement("type")]
        public string DataType { get; set; }

        [BsonElement("name")]
        public string Name { get; set; }

        [BsonElement("value")]
        public string Value { get; set; }
    }
}
