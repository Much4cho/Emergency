using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System;

namespace Tests.DataGenerator.Models
{
    public class DataContext : DbContext
    {
        public DbSet<Models.Emergency> Emergencies { get; set; }
        public DbSet<EmergencyType> EmergencyTypes{ get; set; }
        public DbSet<Emergency_Team> Emergencies_Teams { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Team> Teams { get; set; }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
                optionsBuilder.UseSqlServer("Server=tcp:emergency.database.windows.net,1433;Initial Catalog=emergency_db;Persist Security Info=False;User ID=tai_agh;Password=SRprojekt2020!;MultipleActiveResultSets=True;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;");
            base.OnConfiguring(optionsBuilder);
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Models.Emergency>().ToTable("EMERGENCIES");
            modelBuilder.Entity<EmergencyType>().ToTable("EMERGENCYTYPES");
        }
    }
}
