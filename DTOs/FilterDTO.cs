using MongoDB.Bson.Serialization.Attributes;
using System.Collections.Generic;

namespace Mohjak.ContactManagement.DTOs
{
    public class FilterDTO
    {
        [BsonElement("fields")]
        public IList<FilterFieldDTO> Fields { get; set; }
    }
}
