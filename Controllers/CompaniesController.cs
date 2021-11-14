using Microsoft.AspNetCore.Mvc;
using Mohjak.ContactManagement.DTOs;
using Mohjak.ContactManagement.Entities;
using Mohjak.ContactManagement.Helpers;
using Mohjak.ContactManagement.Models;
using Mohjak.ContactManagement.Services;
using MongoDB.Driver;
using System.Collections.Generic;

namespace Mohjak.ContactManagement.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CompaniesController : ControllerBase
    {
        private readonly CompanyService _companyService;

        public CompaniesController(CompanyService companyService)
        {
            _companyService = companyService;
        }

        [HttpGet]
        public ActionResult<List<Company>> Get() =>
            _companyService.Get();

        [HttpGet("{id:length(24)}", Name = "GetCompany")]
        public ActionResult<Company> Get(string id)
        {
            var company = _companyService.Get(id);
            if (company == null)
            {
                return NotFound();
            }

            return company;
        }

        [HttpPost]
        public ActionResult<Company> Create(CompanyDTO company)
        {
            try
            {
                var createdCompany = _companyService.Create(company);

                return CreatedAtRoute("GetCompany", new { id = createdCompany.Id.ToString() }, createdCompany);
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

            return BadRequest(new Message { Text = "An error occurred trying to insert company document." });
        }

        [HttpPut("{id:length(24)}")]
        public IActionResult Update(string id, CompanyDTO companyDTO)
        {
            var contact = _companyService.Get(id);

            if (contact == null)
            {
                return NotFound();
            }

            _companyService.Update(id, companyDTO);

            return NoContent();
        }

        [HttpDelete("{id:length(24)}")]
        public IActionResult Delete(string id)
        {
            var contact = _companyService.Get(id);

            if (contact == null)
            {
                return NotFound();
            }

            _companyService.Remove(contact.Id);

            return NoContent();
        }
    }
}
