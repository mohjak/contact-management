using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System.Collections.Generic;

namespace Mohjak.ContactManagement.DTOs
{
    public class ContactDTO : BaseDTO
    {
        [BsonElement("name")]
        public string Name { get; set; }

        [BsonElement("companies")]
        public IList<string> Companies { get; set; }
    }
}
