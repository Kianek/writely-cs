using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using writely.Models.Dto;
using writely.Services;

namespace writely.Controllers
{
    [ApiController]
    [Route("api/users/{userId}/journals/{journalId}/entries")]
    public class EntriesController
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
            throw new NotImplementedException();
        }

        [HttpGet("{entryId}")]
        public async Task<ActionResult> GetOne(long entryId)
        {
            throw new NotImplementedException();
        }

        [HttpPost]
        public async Task<ActionResult> Add(long journalId, [FromBody] EntryDto entryDto)
        {
            throw new NotImplementedException();
        }

        [HttpDelete("{entryId}")]
        public async Task<ActionResult> Delete(long journalId, long entryId)
        {
            throw new NotImplementedException();
        }

        [HttpPatch("{entryId}")]
        public async Task<ActionResult> Update(EntryDto updatedEntry)
        {
            throw new NotImplementedException();
        }
    }
}