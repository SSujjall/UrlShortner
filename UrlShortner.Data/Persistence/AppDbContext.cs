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
        public DbSet<ApiUser> ApiUsers { get; set; }
        public DbSet<ApiKey> ApiKeys { get; set; }
        public DbSet<UserOtp> UserOtps { get; set; }
    }
}
