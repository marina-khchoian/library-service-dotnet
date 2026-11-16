using LibraryService.DTOs.Auth;

namespace LibraryService.Services.Interfaces
{
    public interface IAuthService
    {
        Task<AuthResponse> RegisterAsync(RegisterRequest registerRequest);
        Task<AuthResponse> LoginAsync(LoginRequest loginRequest);
    }
}
