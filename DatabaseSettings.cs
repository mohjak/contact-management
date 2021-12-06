namespace Mohjak.ContactManagement
{
    public class DatabaseSettings : IDatabaseSettings
    {
        public string ListingsCollectionName { get; set; }
        public string FieldsCollectionName { get; set; }
        public string ConnectionString { get; set; }
        public string DatabaseName { get; set; }
    }

    public interface IDatabaseSettings
    {
        string ListingsCollectionName { get; set; }
        string FieldsCollectionName { get; set; }
        string ConnectionString { get; set; }
        string DatabaseName { get; set; }
    }
}
