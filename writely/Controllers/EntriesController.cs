using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using writely.Models.Dto;
using writely.Services;

namespace writely.Controllers
{
    [ApiController]
    [Route("api/users/{userId}/journals/{journalId}/entries")]
    public class EntriesController : ControllerBase
    {
        private ILogger<EntriesController> _logger;
        private IEntryService _service;

        public EntriesController(ILogger<EntriesController> logger, IEntryService service)
        {
            _logger = logger;
            _service = service;
        }

        [HttpGet]
        public async Task<ActionResult> GetAll(long journalId)
        {
            var result = await _service.GetAllByJournal(journalId);
            if (result == null)
            {
                _logger.LogInformation($"Unable to retrieve entries for journal: {journalId}");
                return NotFound();
            }
            
            _logger.LogInformation($"Entries retrieved");
            return Ok(result);
        }

        [HttpGet("{entryId}")]
        public async Task<ActionResult> GetOne(long entryId)
        {
            var result = await _service.GetById(entryId);
            if (result == null)
            {
                _logger.LogInformation($"Unable to retrieve entry: {entryId}");
                return NotFound();
            }
            
            _logger.LogInformation($"Entry retrieved: {entryId}");
            return Ok(result);
        }

        [HttpPost]
        public async Task<ActionResult> Add(string userId, long journalId, EntryDto entryDto)
        {
            var result = await _service.Add(journalId, entryDto);
            if (result == null)
            {
                _logger.LogInformation($"Unable to add entry: {entryDto.Title}");
                return BadRequest();
            }
            
            _logger.LogInformation($"Entry added: {entryDto.Id}");
            return Created($"api/users/{userId}/journals/{journalId}/entries", result);
        }

        [HttpPatch("{entryId}")]
        public async Task<ActionResult> Update(EntryDto updatedEntry)
        {
            var result = await _service.Update(updatedEntry);
            if (result == null)
            {
                _logger.LogInformation($"Unable to update entry");
                return BadRequest();
            }
            
            _logger.LogInformation($"Entry updated: {updatedEntry.Id}");
            return Ok(result);
        }

        [HttpDelete("{entryId}")]
        public async Task<ActionResult> Delete(long journalId, long entryId)
        {
            await _service.Delete(journalId, entryId);
            _logger.LogInformation($"Deleted entry: {entryId}");
            return Ok();
        }
    }
}