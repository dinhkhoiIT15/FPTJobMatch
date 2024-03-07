using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using FPTJobMatch.Models;

namespace FPTJobMatch.Models
{
    public class DB1670Context : IdentityDbContext
    {
        public DB1670Context(DbContextOptions<DB1670Context>options) : base(options) { }

        public DbSet<Category> Categories { get; set; }
        public DbSet<Job> Jobs { get; set; }
        public DbSet<FPTJobMatch.Models.Profile> Profile { get; set; } = default!;
        public DbSet<FPTJobMatch.Models.ProJob> ProJob { get; set; } = default!;
    }
}
