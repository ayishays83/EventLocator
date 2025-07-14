using EventLocator.Models;
using Microsoft.EntityFrameworkCore;

namespace EventLocator.Models
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }
        public DbSet<EventPlanner> EventPlanners { get; set; }
    }
}



