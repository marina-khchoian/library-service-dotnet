namespace LibraryService.Entities
{
    public class BorrowRecord
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int BookId { get; set; }
        public User User { get; set; } = null!;
        public Book Book { get; set; } = null!;
        public DateTime BorrowedAt { get; set; } = DateTime.UtcNow;
        public DateTime DueDate { get; set; } = DateTime.UtcNow;
        public DateTime? ReturnedAt { get; set; }
    }
}
