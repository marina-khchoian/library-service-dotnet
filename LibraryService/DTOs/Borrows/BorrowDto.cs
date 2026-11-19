namespace LibraryService.DTOs.Borrows
{
    public class BorrowDto
    {
        public int Id { get; set; }
        public int BookId { get; set; }
        public string BookTitle { get; set; } = string.Empty;
        public int UserId { get; set; }
        public string ReaderName { get; set; } = string.Empty;
        public DateTime BorrowedAt { get; set; }
        public DateTime DueDate { get; set; }
        public DateTime? ReturnedAt { get; set; }
        public bool IsOverdue { get; set; }
        public int DaysOverdue { get; set; }
        public decimal FineAmount { get; set; }
        public bool FinePaid { get; set; }
    }
}