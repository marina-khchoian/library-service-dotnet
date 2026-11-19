using LibraryService.DTOs.Borrows;
using LibraryService.DTOs.Common;

namespace LibraryService.Services.Interfaces
{
    public interface IBorrowService
    {
        Task<BorrowDto> BorrowAsync(int userId, BorrowRequest request);
        Task<BorrowDto> ReturnAsync(int userId, int borrowId);
        Task<PagedResult<BorrowDto>> GetMyBorrowsAsync(int userId, MyBorrowsQueryParameters query);
        Task<PagedResult<BorrowDto>> GetAllAsync(BorrowQueryParameters query);
        Task<BorrowDto> PayFineAsync(int borrowId);
    }
}