using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System.Collections.Generic;

namespace Mohjak.ContactManagement.Entities
{
    public class Contact : BaseEntity
    {
        [BsonElement("name")]
        public string Name { get; set; }

        [BsonElement("companies")]
        public IList<ObjectId> Companies { get; set; }

        [BsonElement("fields")]
        public IDictionary<string, object> Fields { get; set; }
    }
}
