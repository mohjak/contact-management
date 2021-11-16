using Microsoft.AspNetCore.Mvc;
using Mohjak.ContactManagement.Entities;
using Mohjak.ContactManagement.DTOs;
using Mohjak.ContactManagement.Services;
using MongoDB.Driver;
using System.Collections.Generic;
using Mohjak.ContactManagement.Models;
using System.Threading.Tasks;

namespace Mohjak.ContactManagement.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ContactsController : ControllerBase
    {
        private readonly ContactService _contactService;

        public ContactsController(ContactService contactService)
        {
            _contactService = contactService;
        }

        [HttpGet]
        public ActionResult<List<Contact>> Get() =>
            _contactService.Get();

        [HttpGet("{id:length(24)}", Name = "GetContact")]
        public ActionResult<Contact> Get(string id)
        {
            var contact = _contactService.Get(id);
            if (contact == null)
            {
                return NotFound();
            }

            return contact;
        }

        [HttpPost]
        public ActionResult<Contact> Create(ContactDTO contact)
        {
            try
            {
                var createdContact = _contactService.Create(contact);

                return CreatedAtRoute("GetContact", new { id = createdContact.Id.ToString() }, createdContact);
            }
            catch (MongoWriteException ex)
            {
                if (ex.WriteError.Category == ServerErrorCategory.DuplicateKey)
                {
                    return BadRequest(new Message
                    {
                        Text = ex.Message
                    });
                }
            }

            return BadRequest(new Message { Text = "An error occurred trying to insert contact document." });
        }

        [HttpPut("{id:length(24)}")]
        public IActionResult Update(string id, ContactDTO contactDTO)
        {
            var contact = _contactService.Get(id);

            if (contact == null)
            {
                return NotFound();
            }

            _contactService.Update(id, contactDTO);

            return NoContent();
        }

        [HttpDelete("{id:length(24)}")]
        public IActionResult Delete(string id)
        {
            var contact = _contactService.Get(id);

            if (contact == null)
            {
                return NotFound();
            }

            _contactService.Remove(contact.Id);

            return NoContent();
        }

        [HttpGet("search")]
         public async Task<IActionResult> Search(string term)
        {
            var result = await _contactService.Search(term);

            return Ok(result);
        }

        [HttpPost("filter")]
        public IActionResult Filter(IList<FieldDTO> fields)
        {
            var result = _contactService.Filter(fields);

            return Ok(result);
        }
    }
}
