using LibraryService.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using LibraryService.DTOs.Auth;

namespace LibraryService.Controllers
{
    [Route("api/auth")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;
        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }
        [HttpPost("register")]
        public async Task<ActionResult<AuthResponse>> Register([FromBody] RegisterRequest registerRequest)
        {
            var response = await _authService.RegisterAsync(registerRequest);
            return StatusCode(StatusCodes.Status201Created, response);
        }
        [HttpPost("login")]
        public async Task<ActionResult<AuthResponse>> Login([FromBody] LoginRequest loginRequest)
        {
            var response = await _authService.LoginAsync(loginRequest);
            return Ok(response);
        }
       
    }
}
