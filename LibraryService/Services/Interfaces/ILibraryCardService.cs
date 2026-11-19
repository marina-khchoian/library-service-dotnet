using LibraryService.DTOs.LibraryCards;

namespace LibraryService.Services.Interfaces
{
    public interface ILibraryCardService
    {
        Task<LibraryCardDto> CreateAsync(int userId);
        Task<LibraryCardDto> GetMyCardAsync(int userId);
    }
}