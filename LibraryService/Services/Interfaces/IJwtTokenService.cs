using LibraryService.Entities;
using LibraryService.DTOs.Auth;
namespace LibraryService.Services.Interfaces
{
    public interface IJwtTokenService
    {
       public AuthResponse GenerateToken(User user);
    }
}
