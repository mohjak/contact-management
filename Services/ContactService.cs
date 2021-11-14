using Mohjak.ContactManagement.Entities;
using Mohjak.ContactManagement.Models;
using MongoDB.Driver;
using System;
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

        public Contact Create(ContactDTO contactDTO)
        {
            var contact = new Contact();
            contact.Id = contactDTO.Id;
            contact.Name = contactDTO.Name;
            contact.Companies = contactDTO.Companies;

            foreach (var field in contactDTO.Fields)
            {
                if (field.Type == "Date")
                {
                    if (DateTime.TryParse(field.Value, out DateTime dateResult))
                    {
                        ((IDictionary<string, object>)contact.Fields)[field.Name] = dateResult;
                    }
                }

                if (field.Type == "Text")
                {
                    ((IDictionary<string, object>)contact.Fields)[field.Name] = field.Value;
                }

                if (field.Type == "Number")
                {
                    if (int.TryParse(field.Value, out int numberResult))
                    {
                        ((IDictionary<string, object>)contact.Fields)[field.Name] = numberResult;
                    }
                }
            }

            _contacts.InsertOne(contact);

            return contact;
        }
    }
}
