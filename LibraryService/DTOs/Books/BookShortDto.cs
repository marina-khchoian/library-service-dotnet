namespace LibraryService.DTOs.Books
{
    public class BookShortDto
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public int PublishedYear { get; set; }
    }
}
