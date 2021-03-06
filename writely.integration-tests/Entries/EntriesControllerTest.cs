﻿using System.Threading.Tasks;
using writely.Models;
using writely.Models.Dto;
using writely.Services;
using Xunit;

namespace writely.integration_tests.Entries
{
    public class EntriesControllerTest : IntegrationTestBase, IClassFixture<WebAppFactory<Startup>>
    {
        private IJournalService _journalService;
        private IEntryService _entryService;
        
        public EntriesControllerTest(WebAppFactory<Startup> factory) : base(factory)
        {
        }

        [Fact]
        public async Task GetAll()
        {
            await ArrangeTest();

            var journal = await _journalService.Add(_user.Id, "My Journal");
            await _entryService.Add(journal.Id, CreateEntryDto(_user, "My entry"));
            await _entryService.Add(journal.Id, CreateEntryDto(_user, "My other entry"));
            await _entryService.Add(journal.Id, CreateEntryDto(_user, "My other other entry"));

            var response = await _client.GetAsync(URL(_user.Id, journal.Id));
            response.EnsureSuccessStatusCode();
        }

        [Fact]
        public async Task GetOne()
        {
            await ArrangeTest();
            
            var journal = await _journalService.Add(_user.Id, "My other Journal");
            await _entryService.Add(journal.Id, CreateEntryDto(_user, "My entry"));

            var response = await _client.GetAsync(URL(_user.Id, 1L, 1L));
            response.EnsureSuccessStatusCode();
        }

        [Fact]
        public async Task Add()
        {
            await ArrangeTest();

            var journal = await _journalService.Add(_user.Id, "Gonna add me an entry");
            var entry = CreateEntryDto(_user, "I am but a lowly entry");

            var response = await _client.PostAsync(URL(_user.Id, journal.Id), entry.AsStringContent());
            response.EnsureSuccessStatusCode();
        }

        [Fact]
        public async Task Update()
        {
            await ArrangeTest();

            var journal = await _journalService.Add(_user.Id, "I have an entry to update...");
            var oldEntry = await _entryService.Add(journal.Id, CreateEntryDto(_user, "Old Title"));
            var updatedEntryDto = CreateEntryDto(_user, "New Title");
            updatedEntryDto.Id = oldEntry.Id;
            updatedEntryDto.JournalId = journal.Id;

            var response = await _client.PatchAsync(URL(_user.Id, journal.Id, oldEntry.Id),
                updatedEntryDto.AsStringContent());
            response.EnsureSuccessStatusCode();
        }

        [Fact]
        public async Task Delete()
        {
            await ArrangeTest();

            var journal = _journalService.Add(_user.Id, "Too many entries. Delete one.");
            var entryToDelete = await _entryService.Add(journal.Id, CreateEntryDto(_user, "Don't you dare delete me."));

            var response = await _client.DeleteAsync(URL(_user.Id, journal.Id, entryToDelete.Id));
            response.EnsureSuccessStatusCode();
        }

        private EntryDto CreateEntryDto(AppUser user, string title)
            => new EntryDto { Title = title, Body = "Lookie here", UserId = user.Id };

        protected override async Task ArrangeTest()
        {
            await base.ArrangeTest();
            _entryService = GetService<IEntryService>();
            _journalService = GetService<IJournalService>();
        }

        private string URL(string userId, long journalId) => $"api/users/{userId}/journals/{journalId}/entries";

        private string URL(string userId, long journalId, long entryId) => $"{URL(userId, journalId)}/{entryId}";
    }
}