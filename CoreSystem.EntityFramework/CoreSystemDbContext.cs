using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace CoreSystem.EntityFramework
{
    public class CoreSystemDbContext : IdentityDbContext<CoreSystemUser, CoreSystemRole, int>
    {
        public DbSet<Contact> Contacts { get; set; }

        public CoreSystemDbContext(DbContextOptions<CoreSystemDbContext> options) : base(options)
        {
        }
    }
}
