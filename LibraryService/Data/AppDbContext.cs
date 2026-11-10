using Microsoft.EntityFrameworkCore;
namespace LibraryService.Data
{
    public class AppDbContext :DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }
        public DbSet<Entities.User> Users { get; set; } = null!;
        public DbSet<Entities.Book> Books { get; set; } = null!;
        public DbSet<Entities.Author> Authors { get; set; } = null!;
        public DbSet<Entities.Genre> Genres { get; set; } = null!;
        public DbSet<Entities.BorrowRecord> BorrowRecords { get; set; } = null!;
        public DbSet<Entities.LibraryCard> LibraryCards { get; set; } = null!;
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);
        }
    }
}
