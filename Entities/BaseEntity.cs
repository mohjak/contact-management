using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson.Serialization.IdGenerators;

namespace Mohjak.ContactManagement.Entities
{
    public class BaseEntity : IEntity
    {
        [BsonIgnoreIfDefault]
        [BsonRepresentation(BsonType.ObjectId)]
        [BsonId(IdGenerator = typeof(StringObjectIdGenerator))]
        public string Id { get; set; }
    }
}
