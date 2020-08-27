using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using writely.Data;
using writely.Models.Dto;

namespace writely.Controllers
{
    [ApiController]
    [Route("api/users/{userId}/journals")]
    public class JournalsController
    {
        private ApplicationDbContext _context;
        private ILogger<JournalsController> _logger;

        public JournalsController(ApplicationDbContext context, ILogger<JournalsController> logger)
        {
            _logger = logger;
            _context = context;
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