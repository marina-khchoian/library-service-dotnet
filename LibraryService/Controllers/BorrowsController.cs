using LibraryService.DTOs.Borrows;
using LibraryService.DTOs.Common;
using LibraryService.Extensions;
using LibraryService.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LibraryService.Controllers
{
    [Route("api/borrows")]
    [ApiController]
    [Authorize]
    public class BorrowsController : ControllerBase
    {
        private readonly IBorrowService _borrowService;

        public BorrowsController(IBorrowService borrowService)
        {
            _borrowService = borrowService;
        }

        [HttpPost]
        [Authorize(Roles = "Reader")]
        public async Task<ActionResult<BorrowDto>> Borrow([FromBody] BorrowRequest request)
        {
            var borrow = await _borrowService.BorrowAsync(User.GetUserId(), request);
            return StatusCode(StatusCodes.Status201Created, borrow);
        }

        [HttpPost("{id:int}/return")]
        [Authorize(Roles = "Reader")]
        public async Task<ActionResult<BorrowDto>> Return(int id)
        {
            var borrow = await _borrowService.ReturnAsync(User.GetUserId(), id);
            return Ok(borrow);
        }

        [HttpGet("me")]
        [Authorize(Roles = "Reader")]
        public async Task<ActionResult<PagedResult<BorrowDto>>> GetMine([FromQuery] MyBorrowsQueryParameters query)
        {
            var borrows = await _borrowService.GetMyBorrowsAsync(User.GetUserId(), query);
            return Ok(borrows);
        }

        [HttpGet]
        [Authorize(Roles = "Librarian")]
        public async Task<ActionResult<PagedResult<BorrowDto>>> GetAll([FromQuery] BorrowQueryParameters query)
        {
            var borrows = await _borrowService.GetAllAsync(query);
            return Ok(borrows);
        }
    }
}