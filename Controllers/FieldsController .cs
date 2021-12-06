using Microsoft.AspNetCore.Mvc;
using Mohjak.ContactManagement.DTOs;
using Mohjak.ContactManagement.Models;
using Mohjak.ContactManagement.Services;
using MongoDB.Driver;
using System.Threading.Tasks;

namespace Mohjak.ContactManagement.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FieldsController : ControllerBase
    {
        private readonly FieldService _fieldService;

        public FieldsController(FieldService fieldService)
        {
            _fieldService = fieldService;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var fields = await _fieldService.Get();

            return Ok(fields);
        }


        [HttpGet("{id:length(24)}")]
        public async Task<ActionResult<FieldDTO>> GetField(string id)
        {
            var field = await _fieldService.Get(id);
            if (field == null)
            {
                return NotFound(
                    new Message
                    {
                        Text = $"Field with id: {id} not found!"
                    }
                );
            }

            return Ok(field);
        }

        [HttpPost]
        public async Task<ActionResult<FieldDTO>> Create(CreateUpdateFieldDTO dto)
        {
            try
            {
                var newField = await _fieldService.Create(dto);

                return CreatedAtAction("GetField", new { id = newField.Id }, newField);
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

            return BadRequest(new Message { Text = "An error occurred trying to insert field document." });
        }

        [HttpPut("{id:length(24)}")]
        public async Task<ActionResult<FieldDTO>> Update(string id, CreateUpdateFieldDTO fieldDTO)
        {
            var field = await _fieldService.Get(id, fieldDTO.ListingId);
            if (field == null)
            {
                return NotFound(
                    new Message
                    {
                        Text = $"Field with id: {id} not found!"
                    }
                );
            }

            var updatedField = await _fieldService.Update(id, fieldDTO);

            return Ok(updatedField);
        }

        [HttpDelete("{id:length(24)}")]
        public async Task<IActionResult> Delete(string id)
        {
            var field = await _fieldService.Get(id);
            if (field == null)
            {
                return NotFound(
                    new Message
                    {
                        Text = $"Field with id: {id} not found!"
                    }
                );
            }

            var removedField = await _fieldService.Delete(field.Id);

            return Ok(removedField);
        }

        [HttpPost("Filter")]
        public async Task<IActionResult> Filter(FilterDTO dto)
        {
            var result = await _fieldService.Filter(dto);

            return Ok(result);
        }
    }
}
