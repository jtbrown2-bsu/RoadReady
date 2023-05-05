using Microsoft.EntityFrameworkCore;
using RoadReady.Models;

namespace RoadReady
{
    public class RoadReadyDbContext : DbContext
    {
        public RoadReadyDbContext()
        {

        }
        public DbSet<Stop> Stops { get; set; }
        public DbSet<Tour> Tours { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Tour>().Navigation(e => e.Stops).AutoInclude();
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseInMemoryDatabase(databaseName: "MyDatabase")
                .EnableSensitiveDataLogging()
                .LogTo(Console.WriteLine, LogLevel.Information);
        }
    }
}
