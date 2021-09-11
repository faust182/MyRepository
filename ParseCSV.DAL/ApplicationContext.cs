using Microsoft.EntityFrameworkCore;

namespace ParseCSV.DAL
{
    class ApplicationContext : DbContext
    {
        public DbSet<Counter> Counters { get; set; }

        public DbSet<Measure> Measures { get; set; }

        public ApplicationContext()
        {
            Database.EnsureCreated();
        }

        public ApplicationContext(DbContextOptions<ApplicationContext> options)
            : base(options)
        {
            Database.EnsureCreated();
        }
    }
}
