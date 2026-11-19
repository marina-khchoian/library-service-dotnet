using LibraryService.DTOs.Books;
using LibraryService.DTOs.Common;
namespace LibraryService.Services.Interfaces
{
    public interface IBookService
    {
        Task<PagedResult<BookDto>> GetAllBooksAsync(BookQueryParameters query);
        Task<BookDto> UpdateBookAsync(int id, UpdateBookRequest request);
        public Task <BookDto> GetBookByIdAsync(int id);

        public Task<BookDto> CreateBookAsync(CreateBookRequest request);
        public Task DeleteBookAsync(int id);



    }
}
