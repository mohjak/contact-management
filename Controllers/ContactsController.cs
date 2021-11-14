﻿using Microsoft.AspNetCore.Mvc;
using Mohjak.ContactManagement.Entities;
using Mohjak.ContactManagement.Models;
using Mohjak.ContactManagement.Services;
using MongoDB.Driver;
using System.Collections.Generic;

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
    }
}
