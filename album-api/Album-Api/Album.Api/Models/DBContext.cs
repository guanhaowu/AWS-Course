using Microsoft.EntityFrameworkCore;

namespace Album.Api.Models
{
    public class DBContext : DbContext
    {
        public DBContext(DbContextOptions<DBContext> options) : base(options) { }

        // Defined in Startup Services section.
        // Connection string template from https://www.connectionstrings.com/postgresql/

        public DbSet<Album> Albums { get; set; }
        
        // Build the table with its relation and constraints.
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Album>(
                a =>
                {
                    a.Property("Id");
                    a.Property(e => e.Name);
                    a.Property(e => e.Artist);
                    a.Property(e => e.ImageUrl);
                    a.HasKey("Id");
                });
        }
    }
}