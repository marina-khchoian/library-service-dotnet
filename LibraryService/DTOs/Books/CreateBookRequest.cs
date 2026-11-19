namespace LibraryService.DTOs.Books
{
    public class CreateBookRequest
    {
        public string Title { get; set; } = string.Empty;
        public string ISBN { get; set; } = string.Empty;
        public int PublishedYear { get; set; }
        public int TotalCopies { get; set; }
        public int AuthorId { get; set; }
        public List<int> GenreIds { get; set; } = new();
    }
}