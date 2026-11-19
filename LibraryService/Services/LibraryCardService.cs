using AutoMapper;
using LibraryService.Data;
using LibraryService.DTOs.LibraryCards;
using LibraryService.Entities;
using LibraryService.Exceptions;
using LibraryService.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace LibraryService.Services
{
    public class LibraryCardService : ILibraryCardService
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;

        public LibraryCardService(AppDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<LibraryCardDto> CreateAsync(int userId)
        {
            var existing = await _context.LibraryCards
                .FirstOrDefaultAsync(c => c.UserId == userId);

            if (existing != null)
            {
                // 1:1 — მეორე ბარათი არ შეიძლება
                if (existing.ExpiresAt > DateTime.UtcNow)
                    throw new ConflictException("You already have an active library card.");

                // ვადაგასული ბარათი — ვაახლებთ, ახალს არ ვქმნით
                existing.IssuedAt = DateTime.UtcNow;
                existing.ExpiresAt = DateTime.UtcNow.AddYears(1);
                await _context.SaveChangesAsync();
                return await GetMyCardAsync(userId);
            }

            // ზრდადი ნომერი: ბოლო Id + 1 → LC-000001
            var lastId = await _context.LibraryCards.MaxAsync(c => (int?)c.Id) ?? 0;

            var card = new LibraryCard
            {
                UserId = userId,
                CardNumber = $"LC-{lastId + 1:D6}",
                IssuedAt = DateTime.UtcNow,
                ExpiresAt = DateTime.UtcNow.AddYears(1)
            };

            _context.LibraryCards.Add(card);
            await _context.SaveChangesAsync();

            return await GetMyCardAsync(userId);
        }

        public async Task<LibraryCardDto> GetMyCardAsync(int userId)
        {
            var card = await _context.LibraryCards
                .Include(c => c.User)
                .FirstOrDefaultAsync(c => c.UserId == userId);

            if (card == null)
                throw new NotFoundException("You don't have a library card yet.");

            return _mapper.Map<LibraryCardDto>(card);
        }
    }
}