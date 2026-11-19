using LibraryService.DTOs.LibraryCards;
using LibraryService.Extensions;
using LibraryService.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LibraryService.Controllers
{
    [Route("api/library-cards")]
    [ApiController]
    [Authorize(Roles = "Reader")]
    public class LibraryCardsController : ControllerBase
    {
        private readonly ILibraryCardService _libraryCardService;

        public LibraryCardsController(ILibraryCardService libraryCardService)
        {
            _libraryCardService = libraryCardService;
        }

        [HttpPost]
        public async Task<ActionResult<LibraryCardDto>> Create()
        {
            var card = await _libraryCardService.CreateAsync(User.GetUserId());
            return CreatedAtAction(nameof(GetMine), card);
        }

        [HttpGet("me")]
        public async Task<ActionResult<LibraryCardDto>> GetMine()
        {
            var card = await _libraryCardService.GetMyCardAsync(User.GetUserId());
            return Ok(card);
        }
    }
}