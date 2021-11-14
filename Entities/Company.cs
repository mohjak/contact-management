using MongoDB.Bson.Serialization.Attributes;

namespace Mohjak.ContactManagement.Entities
{
    public class Company : BaseEntity
    {
        [BsonElement("name")]
        public string Name { get; set; }


        [BsonElement("numberOfEmployees")]
        public int NumberOfEmployees { get; set; }
    }
}
