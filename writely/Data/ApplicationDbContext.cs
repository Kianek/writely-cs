using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using writely.Models;

namespace writely.Data
{
    public class ApplicationDbContext : IdentityDbContext<AppUser>
    {
        public DbSet<Journal> Journals { get; set; }
        public DbSet<Entry> Entries { get; set; }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            
            builder.Entity<AppUser>(u =>
            {
                u.Property(usr => usr.FirstName)
                    .IsRequired()
                    .HasMaxLength(255);
                
                
                u.Property(usr => usr.LastName)
                    .IsRequired()
                    .HasMaxLength(255);
            });

            builder.Entity<Journal>(j =>
            {
                j.Property(journal => journal.Title)
                    .HasMaxLength(255)
                    .IsRequired();

                j.Property(journal => journal.Id)
                    .IsRequired();

                j.Property(journal => journal.UserId)
                    .IsRequired();

                j
                    .HasMany(journal => journal.Entries)
                    .WithOne(entry => entry.Journal);
            });

            builder.Entity<Entry>(entry =>
            {
                entry.Property(e => e.Title)
                    .HasMaxLength(255)
                    .IsRequired();

                entry.Property(e => e.Body)
                    .HasMaxLength(3000)
                    .IsRequired();

                entry.Property(e => e.UserId)
                    .IsRequired();

                entry.Property(e => e.Username)
                    .IsRequired();

                entry
                    .HasOne(e => e.Journal)
                    .WithMany(j => j.Entries);
            });
        }
    }
}
