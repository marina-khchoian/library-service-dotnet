using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using LibraryService.DTOs.Auth;
using LibraryService.Entities;
using LibraryService.Services.Interfaces;
using Microsoft.IdentityModel.Tokens;

namespace LibraryService.Services
{
    public class JwtTokenService : IJwtTokenService
    {
        private readonly IConfiguration _configuration;

        public JwtTokenService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public AuthResponse GenerateToken(User user)
        {
            // 1. ვადა ერთხელ ითვლება, რომ ტოკენსა და პასუხში ერთნაირი იყოს
            var expires = DateTime.UtcNow.AddHours(8);

            // 2. რას ვწერთ ტოკენში: ვინ არის (sub), ელფოსტა, როლი
            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
                new Claim(JwtRegisteredClaimNames.Email, user.Email),
                new Claim(ClaimTypes.Role, user.Role.ToString())
            };

            // 3. საიდუმლო გასაღები appsettings.json-იდან (! = ვიცით, რომ არსებობს)
            var key = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]!));

            // 4. ხელმოწერის წესი: ეს გასაღები + HmacSha256 ალგორითმი
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            // 5. ტოკენის აწყობა ყველა ნაწილისგან
            var token = new JwtSecurityToken(
                issuer: _configuration["Jwt:Issuer"],
                audience: _configuration["Jwt:Audience"],
                claims: claims,
                expires: expires,
                signingCredentials: creds);

            // 6. ტოკენი ტექსტად გადაიქცევა და ვადასთან ერთად ბრუნდება
            return new AuthResponse
            {
                AccessToken = new JwtSecurityTokenHandler().WriteToken(token),
                ExpiresAt = expires
            };
        }
    }
}