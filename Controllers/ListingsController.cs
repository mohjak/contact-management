using Microsoft.AspNetCore.Mvc;
using Mohjak.ContactManagement.DTOs;
using Mohjak.ContactManagement.Models;
using Mohjak.ContactManagement.Services;
using MongoDB.Driver;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Mohjak.ContactManagement.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Consumes("application/json")]
    public class ListingsController : ControllerBase
    {
        private readonly ListingService _listingService;

        public ListingsController(ListingService listingService)
        {
            _listingService = listingService;
        }

        [HttpGet]
        public async Task<ActionResult<IList<ListingDTO>>> Get()
        {
            var listings = await _listingService.Get();

            return Ok(listings);
        }
        
        [HttpGet("{id:length(24)}")]
        public async Task<ActionResult<ListingDTO>> GetListing(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return BadRequest(
                    new Message
                    {
                        Text = "Invalid listing Id."
                    }
                );
            }

            var listing = await _listingService.Get(id);
            if (listing == null)
            {
                return NotFound(
                    new Message
                    { 
                        Text = $"Listing with id: {id} not found!"
                    }
                );
            }

            return Ok(listing);
        }

        [HttpPost]
        public async Task<ActionResult<ListingDTO>> Create(CreateUpdateListingDTO dto)
        {
            try
            {
                var newListing = await _listingService.Create(dto);

                return CreatedAtAction("GetListing", new { id = newListing.Id }, newListing);
            }
            catch (MongoWriteException ex)
            {
                if (ex.WriteError.Category == ServerErrorCategory.DuplicateKey)
                {
                    return BadRequest(
                        new Message
                        {
                            Text = ex.Message
                        }
                    );
                }
            }

            return BadRequest(new Message { Text = "An error occurred trying to insert listing." });
        }

        [HttpPut("{id:length(24)}")]
        public async Task<ActionResult<ListingDTO>> Update(string id, CreateUpdateListingDTO dto)
        {
            if (string.IsNullOrEmpty(id))
            {
                return BadRequest(
                    new Message
                    {
                        Text = "Invalid listing Id."
                    }
                );
            }

            try
            {
                var listing = await _listingService.Get(id);
                if (listing == null)
                {
                    return NotFound(
                        new Message
                        {
                            Text = $"Listing with id: {id} not found!"
                        }
                    );
                }
                var updatedListing = await _listingService.Update(id, dto);

                return Ok(updatedListing);
            }
            catch (MongoWriteException ex)
            {
                if (ex.WriteError.Category == ServerErrorCategory.DuplicateKey)
                {
                    return BadRequest(
                        new Message
                        {
                            Text = ex.Message
                        }
                    );
                }
            }

            return BadRequest(new Message { Text = "An error occurred trying to upate listing." });
        }
        
        [HttpDelete("{id:length(24)}")]
        public async Task<IActionResult> Delete(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return BadRequest(
                    new Message { 
                        Text = "Invalid listing Id." 
                    }
                );
            }

            var listing = await _listingService.Get(id);
            if (listing == null)
            {
                return NotFound(
                    new Message
                    {
                        Text = $"Listing with id: {id} not found!"
                    }
                );
            }

            var deletedListing = await _listingService.Delete(id);

            return Ok(deletedListing);
        }
    }
}
