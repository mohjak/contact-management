using MongoDB.Bson.Serialization.Attributes;
using System.Collections.Generic;

namespace Mohjak.ContactManagement.DTOs
{
    public class ListingDTO : BaseDTO
    {
        [BsonElement("listings")]
        public IList<string> Listings { get; set; } = new List<string>();

        [BsonElement("fields")]
        public IList<string> Fields { get; set; } = new List<string>();
    }
}
