using LibraryService.DTOs.Books;
namespace LibraryService.DTOs.Authors
{
    public class AuthorDetailsDto
    {
        public int Id { get; set; }
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public int BirthYear { get; set; }
        public string Country { get; set; } = string.Empty;
        public int BooksCount { get; set; }
        public List<BookShortDto> Books { get; set; } = new ();
    }
}
