using LibraryService.DTOs.Common;
using LibraryService.DTOs.Authors;

namespace LibraryService.Services.Interfaces
{
    public interface IAuthorService
    {
        Task<PagedResult<AuthorDto>> GetAllAsync(AuthorQueryParameters query);
        Task<AuthorDetailsDto> GetByIdAsync(int id);
        Task<AuthorDto> CreateAsync(CreateAuthorRequest createAuthorRequest);
        Task<AuthorDto> UpdateAsync(int id,UpdateAuthorRequest updateAuthorRequest);
        Task DeleteAsync(int id);
    }
}
