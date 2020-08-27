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
        public async Task<ActionResult<JournalDto>> GetOne(long journalId)
        {
            throw new NotImplementedException();
        }

        [HttpGet("{page?}")]
        public async Task<ActionResult<List<JournalDto>>> GetAll(string userId, int page)
        {
            throw new NotImplementedException();
        }

        [HttpPost("{title}")]
        public async Task<ActionResult<JournalDto>> Add(string userId, string title)
        {
            throw new NotImplementedException();
        }

        [HttpPatch("{journalId}")]
        public async Task<ActionResult<JournalDto>> Update(long journalId, [FromBody] JournalDto journal)
        {
            throw new NotImplementedException();            
        }

        [HttpDelete("{journalId}")]
        public async Task<ActionResult> Delete(long journalId)
        {
            throw new NotImplementedException();
        }
    }
}