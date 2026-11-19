using AutoMapper;
using LibraryService.Data;
using LibraryService.DTOs.Borrows;
using LibraryService.DTOs.Common;
using LibraryService.Entities;
using LibraryService.Exceptions;
using LibraryService.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace LibraryService.Services
{
    public class BorrowService : IBorrowService
    {
        private const int MaxActiveBorrows = 3;
        private const int BorrowDays = 14;

        private readonly AppDbContext _context;
        private readonly IMapper _mapper;

        public BorrowService(AppDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<BorrowDto> BorrowAsync(int userId, BorrowRequest request)
        {
            var now = DateTime.UtcNow;

            // წესი 1ა: აქტიური (ვადაგაუსვლელი) ბარათი
            var hasActiveCard = await _context.LibraryCards
                .AnyAsync(c => c.UserId == userId && c.ExpiresAt > now);
            if (!hasActiveCard)
                throw new BusinessRuleException("You need an active library card to borrow books.");

            var book = await _context.Books.FindAsync(request.BookId);
            if (book == null)
                throw new NotFoundException("Book not found");

            // წესი 1ბ: ხელმისაწვდომი ეგზემპლარი
            if (book.AvailableCopies <= 0)
                throw new BusinessRuleException("No available copies of this book.");

            // წესი 1გ: 3-ზე ნაკლები აქტიური გაცემა
            var activeCount = await _context.BorrowRecords
                .CountAsync(r => r.UserId == userId && r.ReturnedAt == null);
            if (activeCount >= MaxActiveBorrows)
                throw new BusinessRuleException($"You cannot have more than {MaxActiveBorrows} active borrows.");

            // წესი 1დ: ეს წიგნი უკვე არ აქვს აღებული
            var alreadyBorrowed = await _context.BorrowRecords
                .AnyAsync(r => r.UserId == userId && r.BookId == request.BookId && r.ReturnedAt == null);
            if (alreadyBorrowed)
                throw new BusinessRuleException("You have already borrowed this book.");

            // წესი 2: AvailableCopies − 1, DueDate = BorrowedAt + 14 დღე
            var record = new BorrowRecord
            {
                UserId = userId,
                BookId = request.BookId,
                BorrowedAt = now,
                DueDate = now.AddDays(BorrowDays)
            };
            book.AvailableCopies--;

            _context.BorrowRecords.Add(record);
            await _context.SaveChangesAsync(); // ორივე ცვლილება ერთ ტრანზაქციაში

            return await GetDtoAsync(record.Id);
        }

        public async Task<BorrowDto> ReturnAsync(int userId, int borrowId)
        {
            var record = await _context.BorrowRecords
                .Include(r => r.Book)
                .FirstOrDefaultAsync(r => r.Id == borrowId);

            if (record == null)
                throw new NotFoundException("Borrow record not found");

            // ownership: მხოლოდ საკუთარი გაცემა
            if (record.UserId != userId)
                throw new ForbiddenException("You can only return your own borrows.");

            // წესი 3: უკვე დაბრუნებული → 400
            if (record.ReturnedAt != null)
                throw new BusinessRuleException("This book has already been returned.");

            record.ReturnedAt = DateTime.UtcNow;
            record.Book.AvailableCopies++;
            await _context.SaveChangesAsync();

            return await GetDtoAsync(record.Id);
        }

        public async Task<PagedResult<BorrowDto>> GetMyBorrowsAsync(int userId, MyBorrowsQueryParameters p)
        {
            var query = _context.BorrowRecords.Where(r => r.UserId == userId);

            if (p.ActiveOnly)
                query = query.Where(r => r.ReturnedAt == null);

            return await ToPagedAsync(query, p.Page, p.PageSize);
        }

        public async Task<PagedResult<BorrowDto>> GetAllAsync(BorrowQueryParameters p)
        {
            IQueryable<BorrowRecord> query = _context.BorrowRecords;

            if (p.UserId.HasValue)
                query = query.Where(r => r.UserId == p.UserId.Value);

            if (p.OverdueOnly)
            {
                var now = DateTime.UtcNow; // ცვლადად, რომ EF-მა SQL პარამეტრად გადასცეს
                query = query.Where(r => r.ReturnedAt == null && r.DueDate < now);
            }

            return await ToPagedAsync(query, p.Page, p.PageSize);
        }

        // ერთი გაცემა Book-ით და User-ით, რომ BookTitle და ReaderName შეივსოს
        private async Task<BorrowDto> GetDtoAsync(int id)
        {
            var record = await _context.BorrowRecords
                .Include(r => r.Book)
                .Include(r => r.User)
                .FirstAsync(r => r.Id == id);
            return _mapper.Map<BorrowDto>(record);
        }

        // საერთო pagination ორივე სიისთვის
        private async Task<PagedResult<BorrowDto>> ToPagedAsync(IQueryable<BorrowRecord> query, int page, int pageSize)
        {
            page = page < 1 ? 1 : page;
            pageSize = pageSize < 1 ? 20 : Math.Min(pageSize, 100);

            var total = await query.CountAsync();

            var records = await query
                .Include(r => r.Book)
                .Include(r => r.User)
                .OrderByDescending(r => r.BorrowedAt)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return new PagedResult<BorrowDto>
            {
                Items = _mapper.Map<List<BorrowDto>>(records),
                TotalCount = total,
                Page = page,
                PageSize = pageSize
            };
        }
    }
}