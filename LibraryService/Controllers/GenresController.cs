using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using LibraryService.Services.Interfaces;
using LibraryService.DTOs.Genres;
namespace LibraryService.Controllers
{
    [Route("api/genres")]
    [Authorize]
    [ApiController]
    public class GenreController : ControllerBase
    {
        private readonly IGenreService _genreService;
        public GenreController(IGenreService genreService)
        {
            _genreService = genreService;
        }
        [HttpPost]
        [Authorize(Roles = "Librarian")]
        public async Task<IActionResult> CreateGenre([FromBody] CreateGenreRequest request)
        {
            var genre = await _genreService.CreateGenreAsync(request);
            return StatusCode(StatusCodes.Status201Created, genre);
        }
        [HttpGet]
        public async Task<IActionResult> GetAllGenres()
        {
            var genres = await _genreService.GetAllGenreAsync();
            return Ok(genres);
        }
    }
}
