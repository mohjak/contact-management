using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Mohjak.ContactManagement.DTOs
{
    public class CompanyDTO : BaseDTO
    {
        [BsonElement("name")]
        public string Name { get; set; }

        [BsonElement("numberOfEmployees")]
        public int NumberOfEmployees { get; set; }

    }
}
