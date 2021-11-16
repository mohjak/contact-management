using Mohjak.ContactManagement.DTOs;
using Mohjak.ContactManagement.Entities;
using Mohjak.ContactManagement.Helpers;
using MongoDB.Bson;
using MongoDB.Driver;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Mohjak.ContactManagement.Services
{
    public class CompanyService
    {
        private readonly IMongoCollection<Company> _companies;

        public CompanyService(IContactManagementDatabaseSettings settings)
        {
            var client = new MongoClient(settings.ConnectionString);
            var database = client.GetDatabase(settings.DatabaseName);

            _companies = database.GetCollection<Company>(settings.CompaniesCollectionName);

            var indexKeysDefinition = Builders<Company>.IndexKeys.Ascending(company => company.Name);
            _companies.Indexes.CreateOne(new CreateIndexModel<Company>(indexKeysDefinition, new CreateIndexOptions<Company>
            {
                Unique = true,
            }));
        }

        public List<Company> Get() =>
            _companies.Find(company => true).ToList();

        public Company Get(string id) =>
            _companies.Find(company => company.Id == id).FirstOrDefault();

        public Company Create(CompanyDTO companyDTO)
        {
            var company = new Company();
            company.Id = companyDTO.Id;
            company.Name = companyDTO.Name;
            company.NumberOfEmployees = companyDTO.NumberOfEmployees;
            company.Fields = FieldsHelper.PopulateFields(companyDTO.Fields);

            _companies.InsertOne(company);

            return company;
        }

        public void Update(string id, CompanyDTO companyDTO)
        {
            var company = Get(id);
            company.Fields = FieldsHelper.PopulateFields(companyDTO.Fields);
            company.NumberOfEmployees = companyDTO.NumberOfEmployees;
            _companies.ReplaceOne(contact => contact.Id == id, company);
        }

        public void Remove(string id) =>
            _companies.DeleteOne(contact => contact.Id == id);

        public async Task<List<Company>> Search(string term)
        {
            var filter = Builders<Company>.Filter.Regex("name", new BsonRegularExpression(".*" + term + ".*"));

            var data = await (await _companies.FindAsync<Company>(filter).ConfigureAwait(false)).ToListAsync();

            return data;
        }

        public IList<Company> Filter(IList<FieldDTO> fields)
        {
            var filter = Builders<Company>.Filter.Empty;

            foreach (var field in fields)
            {
                if (!field.IsExisting)
                {
                    filter &= Builders<Company>.Filter.Eq(x => x.Fields[field.Name], field.Value);
                }
                else
                {
                    if (field.Name == "Name")
                    {
                        filter &= Builders<Company>.Filter.Eq(x => x.Name, field.Value);                        
                    }
                    if (field.Name == "NumberOfEmployees")
                    {
                        filter &= Builders<Company>.Filter.Eq(x => x.NumberOfEmployees, int.Parse(field.Value));
                    }
                }
            }

            var data = _companies.Find(filter).ToList();

            return data;
        }
    }
}
