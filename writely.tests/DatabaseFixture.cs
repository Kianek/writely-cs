using System;
using System.Data.Common;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using writely.Data;

namespace writely.tests
{
    public class DatabaseFixture : IDisposable
    {
        public DbConnection Connection { get; }

        public DatabaseFixture()
        {
            // Suppress foreign key requirement
            Connection = new SqliteConnection("DataSource=:memory:;Foreign Keys=false");
            Connection.Open();
        }

        public ApplicationDbContext CreateContext()
        {
            var context = new ApplicationDbContext(
                new DbContextOptionsBuilder<ApplicationDbContext>()
                    .UseSqlite(Connection)
                    .Options);

            context.Database.EnsureCreated();
            context.Database.EnsureDeleted();

            return context;
        }

        public void Dispose() => Connection.Dispose();
    }
}