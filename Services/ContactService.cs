using Mohjak.ContactManagement.Entities;
using Mohjak.ContactManagement.Models;
using MongoDB.Driver;
using System.Collections.Generic;
using System.Linq;


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

        public Contact Get(string id) =>
            _contacts.Find(contact => contact.Id == id).FirstOrDefault();

        public Contact Create(Contact contact)
        {
            _contacts.InsertOne(contact);

            return contact;
        }
    }
}
