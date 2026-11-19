using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace LibraryService.Extensions
{
    public static class ClaimsPrincipalExtensions
    {
        public static int GetUserId(this ClaimsPrincipal user)
        {
            // .NET 8-ში JwtBearer "sub"-ს ავტომატურად NameIdentifier-ად გადაარქმევს
            var value = user.FindFirstValue(ClaimTypes.NameIdentifier)
                        ?? user.FindFirstValue(JwtRegisteredClaimNames.Sub);
            return int.Parse(value!);
        }
    }
}