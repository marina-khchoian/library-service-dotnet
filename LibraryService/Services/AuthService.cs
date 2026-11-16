
using LibraryService.Data;
using LibraryService.DTOs.Auth;
using LibraryService.Entities;
using LibraryService.Exceptions;
using LibraryService.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
namespace LibraryService.Services
{
    public class AuthService : IAuthService
    {
        private readonly AppDbContext context;
        private readonly IJwtTokenService jwtTokenService;
        public AuthService(AppDbContext context, IJwtTokenService jwtTokenService)
        {
            this.context = context;
            this.jwtTokenService = jwtTokenService;
        }
        public async Task<AuthResponse > RegisterAsync(RegisterRequest registerRequest)
        {
            var emailExists = await context.Users.AnyAsync(u => u.Email == registerRequest.Email);
            if (emailExists)
            {
                throw new ConflictException("User already exists");
            }       

            var user = new User
            {
                Email = registerRequest.Email,
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(registerRequest.Password),
                CreatedAt = DateTime.UtcNow,
                FullName = registerRequest.FullName,
                Role = registerRequest.Role
            };
            context.Users.Add(user);
            await context.SaveChangesAsync();
            return jwtTokenService.GenerateToken(user);

        }

        public async Task<AuthResponse> LoginAsync(LoginRequest loginRequest)
        {
            var user = await context.Users.FirstOrDefaultAsync(u => u.Email == loginRequest.Email);
            if (user == null || !BCrypt.Net.BCrypt.Verify(loginRequest.Password, user.PasswordHash))
            {
                throw new UnauthorizedException("Invalid email or password");
            }
            return jwtTokenService.GenerateToken(user);
        }
    }
}
