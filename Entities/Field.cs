using Mohjak.ContactManagement.Entities.Enums;
using MongoDB.Bson.Serialization.Attributes;

namespace Mohjak.ContactManagement.Entities
{
    public class Field
    {
        [BsonElement("type")]
        public FieldType Type { get; set; }

        [BsonElement("name")]
        public string Name { get; set; }
    }
}
