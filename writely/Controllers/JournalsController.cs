using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using writely.Models.Dto;
using writely.Services;

namespace writely.Controllers
{
    [ApiController]
    [Route("api/users/{userId}/journals")]
    public class JournalsController : ControllerBase
    {
        private IJournalService _service;
        private ILogger<JournalsController> _logger;

        public JournalsController(IJournalService service, ILogger<JournalsController> logger)
        {
            _service = service;
            _logger = logger;
        }

        [HttpGet("{journalId}")]
        public async Task<ActionResult> GetOne(long journalId)
        {
            var journal = await _service.GetById(journalId);
            if (journal == null)
            {
                _logger.LogInformation($"Unable to locate journal: {journalId}");
                return NotFound();
            }
            
            _logger.LogInformation($"{journal.Id} {journal.Title} located");
            return Ok(journal);
        }

        [HttpGet("{page?}")]
        public async Task<ActionResult> GetAll(string userId, int page = 0)
        {
            var journals = await _service.GetAll(userId, page);
            if (journals == null)
            {
                _logger.LogInformation("Unable to retrieve journals");
                return NotFound();
            }

            _logger.LogInformation("Journal/s retrieved");
            return Ok(journals);
        }

        [HttpPost("{title}")]
        public async Task<ActionResult> Add(string userId, string title)
        {
            var journal = await _service.Add(userId, title);
            if (journal == null)
            {
                _logger.LogInformation($"Unable to add journal with title: {title}");
                return BadRequest();
            }

            _logger.LogInformation($"Journal added: {title}");
            return Ok(journal);
        }

        [HttpPatch("{journalId}")]
        public async Task<ActionResult> Update(long journalId, [FromBody] JournalDto journal)
        {
            var result = await _service.Update(journal);
            if (result == null)
            {
                _logger.LogInformation("Unable to retrieve and update journal");
                return BadRequest();
            }
            
            _logger.LogInformation($"Updated journal: {journalId}");
            return Ok(result);
        }

        [HttpDelete("{journalId}")]
        public async Task<ActionResult> Delete(long journalId)
        {
            await _service.Delete(journalId);
            _logger.LogInformation($"Journal deleted: {journalId}");
            return Ok();
        }
    }
}