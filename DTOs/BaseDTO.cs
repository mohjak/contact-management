using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Mohjak.ContactManagement.DTOs
{
    public class BaseDTO
    {
        [BsonIgnoreIfDefault]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }
    }
}
