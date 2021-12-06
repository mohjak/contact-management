using Mohjak.ContactManagement.Entities.Enums;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Mohjak.ContactManagement.Entities
{
    public class Field : BaseEntity
    {
        [BsonRepresentation(BsonType.ObjectId)]
        public string ListingId { get; set; }

        [JsonConverter(typeof(StringEnumConverter))]
        [BsonRepresentation(BsonType.String)]
        public DataType DataType { get; set; }
        public string Name { get; set; }

        public string Value { get; set; }

        public bool Required { get; set; } = false;
    }
}
