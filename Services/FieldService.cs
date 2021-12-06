using Mohjak.ContactManagement.DTOs;
using Mohjak.ContactManagement.Entities;
using MongoDB.Driver;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Mohjak.ContactManagement.Services
{
    public class FieldService
    {
        private readonly IMongoCollection<Field> _fields;
        private readonly IMongoCollection<Listing> _companies;

        public FieldService(IDatabaseSettings settings)
        {
            var client = new MongoClient(settings.ConnectionString);
            var database = client.GetDatabase(settings.DatabaseName);

            _fields = database.GetCollection<Field>(settings.FieldsCollectionName);
            _companies = database.GetCollection<Listing>(settings.ListingsCollectionName);

            var indexKeysDefinition = Builders<Field>.IndexKeys.Combine(
                Builders<Field>.IndexKeys.Ascending(f => f.ListingId),
                Builders<Field>.IndexKeys.Ascending(f => f.Name)
            );
            _fields.Indexes.CreateOne(new CreateIndexModel<Field>(indexKeysDefinition,
                new CreateIndexOptions<Field>
                {
                    Unique = true,
                }
            ));
        }

        public async Task<FieldDTO> Create(CreateUpdateFieldDTO dto)
        {
            var field = new Field();
            field.ListingId = dto.ListingId;
            field.DataType = dto.DataType;
            field.Name = dto.Name;
            field.Value = dto.Value;

            await _fields.InsertOneAsync(field);

            return ToDTO(field);
        }

        public async Task<IList<FieldDTO>> Get()
        {
            var fields = await _fields.FindAsync(field => true);
            var dto = ToDTOs(fields.ToList());

            return dto;
        }

        public async Task<FieldDTO> Get(string fieldId)
        {
            if (string.IsNullOrEmpty(fieldId)) return null;
            var field = await _fields.FindAsync(f => f.Id == fieldId);
            var dto = ToDTO(field.FirstOrDefault());

            return dto;
        }

        public async Task<FieldDTO> Get(string fieldId, string listingId)
        {
            if (string.IsNullOrEmpty(fieldId) || string.IsNullOrEmpty(listingId)) return null;

            var field = await _fields.FindAsync(f => f.Id == fieldId && f.ListingId == listingId);
            var dto = ToDTO(field.FirstOrDefault());

            return dto;
        }

        public async Task<FieldDTO> Update(string fieldId, CreateUpdateFieldDTO dto)
        {
            var updateAction = Builders<Field>.Update
                                 .Set(f => f.Name, dto.Name)
                                 .Set(f => f.Value, dto.Value)
                                 .Set(f => f.DataType, dto.DataType);

            await _fields.UpdateOneAsync(f => f.Id == fieldId, updateAction);
            var updatedFıeld = await _fields.FindAsync(f => f.Id == fieldId);

            return ToDTO(await updatedFıeld.FirstOrDefaultAsync());
        }


        public async Task<FieldDTO> Delete(string fieldId)
        {
            var deletedField = await _fields.FindAsync(f => f.Id == fieldId);
            await _fields.DeleteOneAsync(f => f.Id == fieldId);

            return ToDTO(await deletedField.FirstOrDefaultAsync());
        }

        public async Task<IList<FieldDTO>> GetByListingId(string listingId)
        {
            if (string.IsNullOrEmpty(listingId)) return null;

            var fields = await _fields.FindAsync(f => f.ListingId == listingId);
            var dto = ToDTOs(fields.ToList());

            return dto;
        }

        public async Task<IList<FieldDTO>> Filter(FilterDTO dto)
        {
            var filter = Builders<Field>.Filter.Empty;
            foreach (var field in dto.Fields)
            {
                filter &= Builders<Field>.Filter.Where(f => f.ListingId == field.ListingId
                    && f.Id == field.Id
                );

                filter &= Builders<Field>.Filter.Eq(f => f.Value, field.Value);
            }
            var filterResult = await _fields.FindAsync(filter);
            var fields = await filterResult.ToListAsync();

            return ToDTOs(fields);
        }

        #region Helpers
        protected IList<FieldDTO> ToDTOs(IList<Field> fields)
        {
            var fieldsDTO = new List<FieldDTO>();
            return fields.Select(c =>
            {
                var dto = ToDTO(c);
                return dto;
            }).ToList();
        }

        protected FieldDTO ToDTO(Field field)
        {
            if (field == null) return null;
            var dto = new FieldDTO();
            dto.Id = field.Id;
            dto.ListingId = field.ListingId;
            dto.DataType = field.DataType;
            dto.Name = field.Name;
            dto.Value = field.Value;

            return dto;
        }
        #endregion
    }
}
