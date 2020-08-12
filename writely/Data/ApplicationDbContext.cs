using System;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using writely.Models;

namespace writely.Data
{
    public class ApplicationDbContext : DbContext
           {
        public DbSet<AppUser> Users { get; set; }
        public DbSet<Journal> Journals { get; set; }
        public DbSet<Entry> Entries { get; set; }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {

        }
    }
}
