using Mohjak.ContactManagement.Entities.Enums;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Mohjak.ContactManagement.DTOs
{
    public class CreateUpdateFieldDTO : BaseDTO
    {
        [BsonElement("listingId")]
        public string ListingId { get; set; }

        [BsonElement("dataType")]
        public DataType DataType { get; set; }

        [BsonElement("name")]
        public string Name { get; set; }

        [BsonElement("value")]
        public string Value { get; set; }

        [BsonElement("required")]
        public bool Required { get; set; }
    }
}
