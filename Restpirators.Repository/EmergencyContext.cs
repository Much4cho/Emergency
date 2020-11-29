using Microsoft.EntityFrameworkCore;
using Restpirators.DataAccess.Entities;

namespace Restpirators.Repository
{
    public class EmergencyContext : DbContext
    {
        public EmergencyContext() : base() { }
        public EmergencyContext(DbContextOptions<EmergencyContext> options) : base(options) { }
        public DbSet<Emergency> Emergencies { get; set; }
        public DbSet<EmergencyType> EmergencyTypes { get; set; }
        public DbSet<Team> Teams { get; set; }
        public DbSet<User> Users { get; set; }

        //// UNCOMMENT FOR MIGRATION
        //protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        //{
        //    optionsBuilder.UseSqlServer(@"Server=tcp:restpiratory.database.windows.net,1433;Initial Catalog=Restpirators;Persist Security Info=False;User ID=aghtim;Password={your_password};MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;");
        //}
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Emergency>().HasOne(m => m.AssignedToTeam).WithMany(m => m.AssignedEmergencies).OnDelete(DeleteBehavior.SetNull);
            modelBuilder.Entity<EmergencyType>().Property(m => m.Name).IsRequired();
            modelBuilder.Entity<User>().Property(m => m.Name).IsRequired();
        }
    }
}
