namespace Mohjak.ContactManagement
{
    public class ContactManagementDatabaseSettings : IContactManagementDatabaseSettings
    {
        public string ContactsCollectionName { get; set; }
        public string CompaniesCollectionName { get; set; }
        public string ConnectionString { get; set; }
        public string DatabaseName { get; set; }
    }

    public interface IContactManagementDatabaseSettings
    {
        string ContactsCollectionName { get; set; }
        string CompaniesCollectionName { get; set; }
        string ConnectionString { get; set; }
        string DatabaseName { get; set; }
    }

}
