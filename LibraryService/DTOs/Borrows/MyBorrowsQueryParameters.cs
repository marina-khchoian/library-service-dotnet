namespace LibraryService.DTOs.Borrows
{
    public class MyBorrowsQueryParameters
    {
        public bool ActiveOnly { get; set; }
        public int Page { get; set; } = 1;
        public int PageSize { get; set; } = 20;
    }
}