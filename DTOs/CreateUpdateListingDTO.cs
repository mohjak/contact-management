using MongoDB.Bson.Serialization.Attributes;
using System.Collections.Generic;

namespace Mohjak.ContactManagement.DTOs
{
    public class CreateUpdateListingDTO
    {
        [BsonElement("listings")]
        public IList<string> Listings { get; set; }

        [BsonElement("fields")]
        public IList<CreateUpdateFieldDTO> Fields { get; set; }
    }
}
