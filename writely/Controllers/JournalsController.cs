using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using writely.Data;
using writely.Models.Dto;
using writely.Services;

namespace writely.Controllers
{
    [ApiController]
    [Route("api/users/{userId}/journals")]
    public class JournalsController
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
                return new BadRequestResult();
            }
            
            _logger.LogInformation($"{journal.Id} {journal.Title} located");
            return new OkObjectResult(journal);
        }

        [HttpGet("{page?}")]
        public async Task<ActionResult> GetAll(string userId, int page = 0)
        {
            var journals = await _service.GetAll(userId, page);
            if (journals == null)
            {
                _logger.LogInformation("Unable to retrieve journals");
                return new BadRequestResult();
            }

            _logger.LogInformation("Journal/s retrieved");
            return new OkObjectResult(journals);
        }

        [HttpPost("{title}")]
        public async Task<ActionResult> Add(string userId, string title)
        {
            var journal = await _service.Add(userId, title);
            if (journal == null)
            {
                _logger.LogInformation($"Unable to add journal with title: {title}");
                return new BadRequestResult();
            }

            _logger.LogInformation($"Journal added: {title}");
            return new OkObjectResult(journal);
        }

        [HttpPatch("{journalId}")]
        public async Task<ActionResult> Update(long journalId, [FromBody] JournalDto journal)
        {
            var result = await _service.Update(journal);
            if (result == null)
            {
                _logger.LogInformation("Unable to retrieve and update journal");
                return new BadRequestResult();
            }
            
            _logger.LogInformation($"Updated journal: {journalId}");
            return new OkObjectResult(result);
        }

        [HttpDelete("{journalId}")]
        public async Task<ActionResult> Delete(long journalId)
        {
            await _service.Delete(journalId);
            _logger.LogInformation($"Journal deleted: {journalId}");
            return new OkResult();
        }
    }
}