namespace LibraryService.DTOs.Borrows
{
    public class BorrowQueryParameters
    {
        public bool OverdueOnly { get; set; }
        public int? UserId { get; set; }
        public int Page { get; set; } = 1;
        public int PageSize { get; set; } = 20;
    }
}