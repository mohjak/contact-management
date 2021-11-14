using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System.Dynamic;

namespace Mohjak.ContactManagement.Entities
{
    public class BaseEntity
    {
        public BaseEntity()
        {
            Fields = new ExpandoObject();
        }

        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        [BsonElement("fields")]
        public ExpandoObject Fields { get; set; }
    }
}
