using LibraryService.DTOs.Genres;
namespace LibraryService.Services.Interfaces
{
    public interface IGenreService
    {
        public Task<GenreDto> CreateGenreAsync(CreateGenreRequest request);
        public Task<List<GenreDto>> GetAllGenreAsync();
    }
}
