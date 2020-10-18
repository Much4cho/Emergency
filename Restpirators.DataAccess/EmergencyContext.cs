using Restpirators.DataAccess.Models;
using Microsoft.EntityFrameworkCore;

namespace Restpirators.DataAccess
{
    class EmergencyContext : DbContext
    {
        public DbSet<Emergency> Blogs { get; set; }
        public DbSet<EmergencyType> Posts { get; set; }
    }
}
