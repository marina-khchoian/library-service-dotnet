using LibraryService.DTOs.Authors;
using LibraryService.DTOs.Common;
using LibraryService.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LibraryService.Controllers
{
    [Route("api/authors")]
    [Authorize]
    [ApiController]
    public class AuthorsController : ControllerBase
    {
        private readonly IAuthorService _authorService;

        public AuthorsController(IAuthorService authorService)
        {
            _authorService = authorService;
        }

  
        [HttpGet]
        public async Task<ActionResult<PagedResult<AuthorDto>>> GetAll([FromQuery] AuthorQueryParameters query)
        {
            var authors = await _authorService.GetAllAsync(query);
            return Ok(authors);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<AuthorDetailsDto>> GetById(int id)
        {
            var author = await _authorService.GetByIdAsync(id);
            return Ok(author);
        }

        [HttpPost]
        [Authorize(Roles = "Librarian")]
        public async Task<ActionResult<AuthorDto>> Create([FromBody] CreateAuthorRequest request)
        {
            var author = await _authorService.CreateAsync(request);
            return CreatedAtAction(nameof(GetById), new { id = author.Id }, author);
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "Librarian")]
        public async Task<ActionResult<AuthorDto>> Update(int id, [FromBody] UpdateAuthorRequest request)
        {
            var author = await _authorService.UpdateAsync(id, request);
            return Ok(author);
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Librarian")]
        public async Task<IActionResult> Delete(int id)
        {
            await _authorService.DeleteAsync(id);
            return NoContent();
        }
    }
}