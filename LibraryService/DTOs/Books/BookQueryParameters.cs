namespace LibraryService.DTOs.Books
{
    public class BookQueryParameters
    {
        public string? Search { get; set; }
        public int? AuthorId { get; set; }
        public int? GenreId { get; set; }
        public bool AvailableOnly { get; set; }
        public int Page { get; set; } = 1;
        public int PageSize { get; set; } = 20;
    }
}