namespace LibraryService.DTOs.Authors
{
    public class CreateAuthorRequest
    {
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public int BirthYear { get; set; }
        public string Country { get; set; } = string.Empty;
    }
}
