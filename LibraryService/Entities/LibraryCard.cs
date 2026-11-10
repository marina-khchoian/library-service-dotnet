using System.Security.Cryptography.X509Certificates;

namespace LibraryService.Entities
{
    public class LibraryCard
    {
       public int Id { get; set; }
        public int UserId { get; set; }
        public User User { get; set; } = null!;
        public string CardNumber { get; set; } = string.Empty;
        public DateTime IssuedAt { get; set; } = DateTime.UtcNow;
        public DateTime ExpiresAt { get; set; } = DateTime.UtcNow;


    }
}
