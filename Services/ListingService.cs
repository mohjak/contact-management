using Mohjak.ContactManagement.DTOs;
using Mohjak.ContactManagement.Entities;
using Mohjak.ContactManagement.Entities.Enums;
using MongoDB.Bson;
using MongoDB.Driver;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Mohjak.ContactManagement.Services
{
    public class ListingService
    {
        private readonly FieldService _fieldService;
        private readonly IDatabaseSettings _settings;
        private readonly IMongoCollection<Field> _fields;
        private readonly IMongoCollection<Listing> _companies;

        public ListingService(IDatabaseSettings settings
            , FieldService fieldService
        )
        {
            _settings = settings;

            var client = new MongoClient(settings.ConnectionString);
            var database = client.GetDatabase(settings.DatabaseName);

            _fields = database.GetCollection<Field>(settings.FieldsCollectionName);
            _companies = database.GetCollection<Listing>(settings.ListingsCollectionName);

            _fieldService = fieldService;
        }

        public async Task<IList<ListingDTO>> Get()
        {
            var listings = await _companies.FindAsync(listing => true);
            var companiesDTO = ToDTOs(listings.ToList());

            return companiesDTO;
        }

        public async Task<ListingDTO> Get(string id)
        {
            var listing = await _companies.FindAsync(c => c.Id == id);
            var listingDTO = ToDTO(listing.FirstOrDefault());

            return listingDTO;
        }

        public async Task<ListingDTO> Create(CreateUpdateListingDTO dto)
        {
            var listing = new Listing();
            listing.Listings = dto.Listings.Count > 0 ? dto.Listings : new List<string>();
            await _companies.InsertOneAsync(listing);

            foreach (var f in dto.Fields)
            {
                var fieldDTO = new CreateUpdateFieldDTO();
                fieldDTO.ListingId = listing.Id;
                fieldDTO.DataType = f.DataType;
                fieldDTO.Name = f.Name;
                fieldDTO.Value = f.Value;
                fieldDTO.Required = f.Required;
                await _fieldService.Create(fieldDTO);
            }

            var fields = await GetFields(listing.Id);
            var updatedListing = await UpdateFields(listing.Id, fields);

            return ToDTO(updatedListing);
        }

        public async Task<ListingDTO> Update(string id, CreateUpdateListingDTO dto)
        {
            if (string.IsNullOrEmpty(id)) return null;

            var listing = await GetListing(id);
            if (listing == null) return null;

            listing.Listings = dto.Listings.Count > 0 ? dto.Listings : listing.Listings;

            foreach (var f in dto.Fields)
            {
                var findResult = await _fields.FindAsync(x => x.Id == f.Id);
                var field = findResult.FirstOrDefault();
                try
                {
                    if (field == null)
                    {
                        field = new Field();
                        field.ListingId = listing.Id;
                        field.DataType = f.DataType;
                        field.Name = f.Name;
                        field.Value = f.Value;
                        field.Required = f.Required;
                        await _fields.InsertOneAsync(field);
                    }
                    else
                    {
                        field.Name = !string.IsNullOrEmpty(f.Name) ? f.Name : field.Name;
                        field.Value = !string.IsNullOrEmpty(f.Value) ? f.Value : field.Value;
                        field.Required = f.Required != field.Required ? f.Required : field.Required;
                        field.DataType = f.DataType != field.DataType ? f.DataType : field.DataType;
                        await _fields.ReplaceOneAsync(x => x.Id == f.Id
                            , field
                            , new ReplaceOptions { IsUpsert = true }
                        );
                    }
                }
                catch (System.Exception ex)
                {
                    throw;
                }
            }

            var fieldIds = await GetFields(id);
            var updatedListing = await UpdateFields(id, fieldIds);

            return ToDTO(updatedListing);
        }

        public async Task<ListingDTO> Delete(string id)
        {
            await DeleteFields(id);
            var listing = await DeleteListing(id);

            return ToDTO(listing);
        }

        public async Task<Listing> GetListing(string id)
        {
            var result = await _companies.FindAsync(c => c.Id == id);

            return result.FirstOrDefault();
        }

        public async Task<Listing> DeleteListing(string id)
        {
            var listing = await GetListing(id);
            await _companies.DeleteOneAsync(c => c.Id == id);

            return listing;
        }

        public async Task<IList<string>> GetFields(string listingId)
        {
            var result = await _fields.FindAsync(f => f.ListingId == listingId);
            var fields = result.ToList().Select(f => f.Id).ToList();

            return fields;
        }

        public async Task<Listing> UpdateFields(string listingId, IList<string> fields)
        {
            var updateAction = Builders<Listing>.Update
                     .Set(c => c.Fields, fields);
            await _companies.UpdateOneAsync(c => c.Id == listingId, updateAction);
            var result = await _companies.FindAsync(c => c.Id == listingId);
            var listing = result.FirstOrDefault();

            return listing;
        }

        public async Task<IList<string>> DeleteFields(string listingId)
        {
            var fields = await GetFields(listingId);
            foreach (var field in fields)
            {
                await _fields.DeleteOneAsync(f => f.Id == listingId);
            }

            return fields;
        }

        /*
        public async Task<IList<FieldDTO>> Filter(FilterDTO dto)
        {
            var result = await _companies.FindAsync(c => c.Id == dto.ListingId);
            var listing = result.FirstOrDefault();
            if (listing == null) return null;

            var filter = Builders<Field>.Filter.Empty;
            foreach (var field in dto.Fields)
            {
                filter &= Builders<Field>.Filter.Where(f => f.ListingId == listing.Id
                    && f.Id == field.FieldId
                );

                filter &= Builders<Field>.Filter.Eq(f => f.Value, field.Value);
            }
            var filterResult = await _fields.FindAsync(filter);
            var fields = await filterResult.ToListAsync();

            return fields;
        }
        */

        #region Helpers
        protected IList<ListingDTO> ToDTOs(IList<Listing> listings)
        {
            var compniesDTO = new List<ListingDTO>();
            return listings.Select(c =>
            {
                var dto = ToDTO(c);
                return dto;
            }).ToList();
        }

        ListingDTO ToDTO(Listing listing)
        {
            if (listing == null) return null;
            var dto = new ListingDTO();
            dto.Id = listing.Id;
            dto.Listings = listing.Listings;
            dto.Fields = listing.Fields;

            return dto;
        }
        #endregion
    }
}
