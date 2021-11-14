using Mohjak.ContactManagement.Entities;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System.Collections.Generic;

namespace Mohjak.ContactManagement.Models
{
    public class ContactDTO
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        [BsonElement("name")]
        public string Name { get; set; }

        [BsonElement("companies")]
        public IList<ObjectId> Companies { get; set; }

        [BsonElement("fields")]
        public IList<CustomField> Fields { get; set; }
    }
}
