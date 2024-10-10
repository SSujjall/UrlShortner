using Microsoft.EntityFrameworkCore;

using UrlShortner.Data.Models;

namespace UrlShortner.Data.Persistence
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }

        public DbSet<Url> Urls { get; set; }
    }
}
