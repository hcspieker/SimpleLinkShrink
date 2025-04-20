using Microsoft.EntityFrameworkCore;

namespace SimpleLinkShrink.Data.Entity
{
    public class LinkDbContext : DbContext
    {
        public DbSet<Shortlink> Shortlinks { get; set; }

        public LinkDbContext(DbContextOptions options) : base(options)
        {
        }
    }
}
