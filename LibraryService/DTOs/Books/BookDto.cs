namespace LibraryService.DTOs.Books
{
    public class BookDto
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string ISBN { get; set; } = string.Empty;
        public int PublishedYear { get; set; }
        public int TotalCopies { get; set; }
        public int AvailableCopies { get; set; }
        public bool IsAvailable { get; set; }

        public string AuthorName { get; set; } = string.Empty;
        public List<string> Genres { get; set; } = new();
    }
}