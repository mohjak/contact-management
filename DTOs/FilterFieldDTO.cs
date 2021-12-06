using MongoDB.Bson.Serialization.Attributes;

namespace Mohjak.ContactManagement.DTOs
{
    public class FilterFieldDTO
    {
        [BsonElement("listingId")]
        public string ListingId { get; set; }

        [BsonElement("id")]
        public string Id { get; set; }

        [BsonElement("value")]
        public string Value { get; set; }
    }
}
