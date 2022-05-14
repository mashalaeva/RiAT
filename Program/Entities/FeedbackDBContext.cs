using Microsoft.EntityFrameworkCore;

namespace Entities
{
    public class FeedbackDBContext : DbContext
    {
        public DbSet<Employee> Employee { get; set; }
        
        public DbSet<Feedback> Feedback { get; set; }

        public FeedbackDBContext()
        {
            Database.EnsureCreated();
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.EnableSensitiveDataLogging();
            optionsBuilder.UseNpgsql("Host=localhost;Port=5432;Database=FeedbackSystemDatabase;Username=postgres;Password=1234");
        }
    }
}