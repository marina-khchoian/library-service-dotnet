using LibraryService.DTOs.Borrows;
using LibraryService.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LibraryService.Controllers
{
    [Route("api/fines")]
    [ApiController]
    [Authorize(Roles = "Librarian")]
    public class FinesController : ControllerBase
    {
        private readonly IBorrowService _borrowService;

        public FinesController(IBorrowService borrowService)
        {
            _borrowService = borrowService;
        }

        [HttpPost("{borrowId:int}/pay")]
        public async Task<ActionResult<BorrowDto>> Pay(int borrowId)
        {
            var borrow = await _borrowService.PayFineAsync(borrowId);
            return Ok(borrow);
        }
    }
}