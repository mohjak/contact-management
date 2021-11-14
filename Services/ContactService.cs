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
    public class ContactService
    {
        private readonly IMongoCollection<Contact> _contacts;

        public ContactService(IContactManagementDatabaseSettings settings)
        {
            var client = new MongoClient(settings.ConnectionString);
            var database = client.GetDatabase(settings.DatabaseName);

            _contacts = database.GetCollection<Contact>(settings.ContactsCollectionName);

            var indexKeysDefinition = Builders<Contact>.IndexKeys.Ascending(contact => contact.Name);
            _contacts.Indexes.CreateOne(new CreateIndexModel<Contact>(indexKeysDefinition, new CreateIndexOptions<Contact>
            {
                Unique = true,
            }));
        }

        public List<Contact> Get() =>
            _contacts.Find(contact => true).ToList();

        public async Task<List<Contact>> Search(string term)
        {
            var filter = Builders<Contact>.Filter.Regex("name", new BsonRegularExpression(".*" + term + ".*"));
            
            var data = await(await _contacts.FindAsync<Contact>(filter).ConfigureAwait(false)).ToListAsync();

            return data;
        }

        public Contact Get(string id) =>
            _contacts.Find(contact => contact.Id == id).FirstOrDefault();

        public Contact Create(ContactDTO contactDTO)
        {
            var contact = new Contact();
            contact.Id = contactDTO.Id;
            contact.Name = contactDTO.Name;
            contact.Companies = ToObjectIds(contactDTO.Companies);
            contact.Fields = FieldsHelper.PopulateFields(contactDTO.Fields);

            _contacts.InsertOne(contact);

            return contact;
        }

        public void Update(string id, ContactDTO contactDTO)
        {
            var contact = Get(id);
            contact.Companies = ToObjectIds(contactDTO.Companies);
            contact.Fields = FieldsHelper.PopulateFields(contactDTO.Fields);
            _contacts.ReplaceOne(contact => contact.Id == id, contact);
        }

        public void Remove(string id) =>
            _contacts.DeleteOne(contact => contact.Id == id);

        // Helper Methods
        public IList<ObjectId> ToObjectIds(IList<string> ids)
        {
            IList<ObjectId> objectIds = new List<ObjectId>();
            foreach (var id in ids)
            {
                objectIds.Add(new ObjectId(id));
            }

            return objectIds;
        }
    }
}
