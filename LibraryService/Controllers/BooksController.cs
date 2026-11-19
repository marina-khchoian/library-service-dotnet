using Microsoft.AspNetCore.Authorization;
using LibraryService.DTOs.Books;
using LibraryService.Services.Interfaces;
using LibraryService.DTOs.Common;
using Microsoft.AspNetCore.Mvc;

namespace LibraryService.Controllers
{
    [Route("api/books")]
    [ApiController]
    [Authorize]
    public class BooksController : ControllerBase
    {
        private readonly IBookService _bookService;

        public BooksController(IBookService bookService)
        {
            _bookService = bookService;
        }

        [HttpGet]
        public async Task<ActionResult<PagedResult<BookDto>>> GetAll([FromQuery] BookQueryParameters query)
        {
            var books = await _bookService.GetAllBooksAsync(query);
            return Ok(books);
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<BookDto>> GetById(int id)
        {
            var book = await _bookService.GetBookByIdAsync(id);
            return Ok(book);
        }

        [HttpPost]
        [Authorize(Roles = "Librarian")]
        public async Task<ActionResult<BookDto>> Create([FromBody] CreateBookRequest request)
        {
            var book = await _bookService.CreateBookAsync(request);
            return CreatedAtAction(nameof(GetById), new { id = book.Id }, book);
        }

        [HttpPut("{id:int}")]
        [Authorize(Roles = "Librarian")]
        public async Task<ActionResult<BookDto>> Update(int id, [FromBody] UpdateBookRequest request)
        {
            var book = await _bookService.UpdateBookAsync(id, request);
            return Ok(book);
        }

        [HttpDelete("{id:int}")]
        [Authorize(Roles = "Librarian")]
        public async Task<IActionResult> Delete(int id)
        {
            await _bookService.DeleteBookAsync(id);
            return NoContent();
        }
    }
}