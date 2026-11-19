namespace LibraryService.DTOs.LibraryCards
{
    public class LibraryCardDto
    {
        public int Id { get; set; }
        public string CardNumber { get; set; } = string.Empty;
        public DateTime IssuedAt { get; set; }
        public DateTime ExpiresAt { get; set; }
        public string OwnerName { get; set; } = string.Empty;
        public bool IsActive { get; set; }
    }
}