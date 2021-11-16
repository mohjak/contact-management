using MongoDB.Bson.Serialization.Attributes;
using System.Collections.Generic;

namespace Mohjak.ContactManagement.Entities
{
    public class Company : BaseEntity
    {
        [BsonElement("name")]
        public string Name { get; set; }

        [BsonElement("numberOfEmployees")]
        public int NumberOfEmployees { get; set; }

        [BsonElement("fields")]
        public IDictionary<string, object> Fields { get; set; }
    }
}
