using Library.DAL.Entities;
using Microsoft.EntityFrameworkCore;

namespace Library.DAL.EF
{
    public class ApplicationContext : DbContext
    {
        public DbSet<Author> Authors { get; set; }
        public DbSet<Book> Books { get; set; }
        public DbSet<BookLoanRecord> BookLoanRecords { get; set; }
        public DbSet<Genre> Genres { get; set; }
        public DbSet<Reader> Readers { get; set; }
        public DbSet<Staff> Staff { get; set; }


        public ApplicationContext(DbContextOptions<ApplicationContext> options):base(options)
        {
            // Database.EnsureDeleted();
            Database.EnsureCreated();
        }
        
        public ApplicationContext()
        {
            Database.EnsureDeleted();
            Database.EnsureCreated();
        }
 
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder
                .UseLazyLoadingProxies()
                .UseNpgsql("Host=localhost;Port=5432;Database=library;Username=postgres;Password=2456")
                ;
        }
    }
}