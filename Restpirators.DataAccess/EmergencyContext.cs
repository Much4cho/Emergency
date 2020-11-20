using Restpirators.DataAccess.Models;
using Microsoft.EntityFrameworkCore;

namespace Restpirators.DataAccess
{
    public class EmergencyContext : DbContext
    {
        public EmergencyContext(DbContextOptions<EmergencyContext> options) : base(options) { }
        public DbSet<Emergency> Emergencies { get; set; }
        public DbSet<EmergencyType> EmergencyTypes { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
        }
    }
}
