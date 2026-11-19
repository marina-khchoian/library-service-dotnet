using AutoMapper;
using AutoMapper.QueryableExtensions;
using LibraryService.Data;
using LibraryService.DTOs.Books;
using LibraryService.DTOs.Common;
using LibraryService.Entities;
using LibraryService.Exceptions;
using LibraryService.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace LibraryService.Services
{
    public class BookService : IBookService
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;
        public BookService(AppDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<BookDto> CreateBookAsync(CreateBookRequest request)
        {
            var book = _mapper.Map<Book>(request);
            await _context.Genres.Where(g => request.GenreIds.Contains(g.Id)).ForEachAsync(g => book.Genres.Add(g));
            _context.Books.Add(book);
            await _context.SaveChangesAsync();

            var loaded = await _context.Books
               .Include(b => b.Author)
               .Include(b => b.Genres)
               .FirstAsync(b => b.Id == book.Id);
            return _mapper.Map<BookDto>(loaded);
        }

        public async Task DeleteBookAsync(int id)
        {
            var book = await _context.Books.FindAsync(id);
            if (book == null)
                throw new NotFoundException("Book not found");
            var hasActiveBorrow = await _context.BorrowRecords
           .AnyAsync(r => r.BookId == id && r.ReturnedAt == null);

            if (hasActiveBorrow)
                throw new BusinessRuleException("Book has active borrows and cannot be deleted.");
            _context.Books.Remove(book);
            await _context.SaveChangesAsync();
        }
        public async Task<PagedResult<BookDto>> GetAllBooksAsync(BookQueryParameters p)
        {
            // 1. დაცვა არასწორი page/pageSize-გან
            var page = p.Page < 1 ? 1 : p.Page;
            var pageSize = p.PageSize < 1 ? 20 : Math.Min(p.PageSize, 100);

            // 2. ჯერ მხოლოდ მოთხოვნას ვაგებთ, ბაზაში ჯერ არაფერი მიდის
            IQueryable<Book> query = _context.Books;

            // 3. ფილტრები, თითო მხოლოდ მაშინ, თუ კლიენტმა გამოგზავნა
            if (!string.IsNullOrWhiteSpace(p.Search))
                query = query.Where(b => b.Title.Contains(p.Search));
            if (p.AuthorId.HasValue)
                query = query.Where(b => b.AuthorId == p.AuthorId.Value);
            if (p.GenreId.HasValue)
                query = query.Where(b => b.Genres.Any(g => g.Id == p.GenreId.Value));
            if (p.AvailableOnly)
                query = query.Where(b => b.AvailableCopies > 0);

            // 4. სულ რამდენია (ფილტრების შემდეგ, pagination-მდე)
            var total = await query.CountAsync();

            // 5. ერთი გვერდი. ProjectTo SQL-ში ითვლის DTO-ს (AuthorName, Genres, IsAvailable)
            var items = await query
                .OrderBy(b => b.Title)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ProjectTo<BookDto>(_mapper.ConfigurationProvider)
                .ToListAsync();

            return new PagedResult<BookDto>
            {
                Items = items,
                TotalCount = total,
                Page = page,
                PageSize = pageSize
            };
        }
        public async Task<BookDto> GetBookByIdAsync(int id)
        {
            var book = await _context.Books
                .Include(b => b.Author)
                .Include(b => b.Genres)
                .FirstOrDefaultAsync(b => b.Id == id);
            if (book == null)
                throw new NotFoundException("Book not found");
            return _mapper.Map<BookDto>(book);
        }
        public async Task<BookDto> UpdateBookAsync(int id, UpdateBookRequest request)
        {
            // 1. წიგნი ჟანრებით
            var book = await _context.Books
                .Include(b => b.Genres)
                .FirstOrDefaultAsync(b => b.Id == id);
            if (book == null)
                throw new NotFoundException("Book not found");

            // 2. ISBN დუბლიკატი, საკუთარი თავის გარდა
            var isbnTaken = await _context.Books
                .AnyAsync(b => b.ISBN == request.ISBN && b.Id != id);
            if (isbnTaken)
                throw new ConflictException("A book with this ISBN already exists.");

            // 3. რამდენი ეგზემპლარია ახლა გაცემული (Map-მდე, სანამ ველები გადაიწერება)
            var borrowed = book.TotalCopies - book.AvailableCopies;

            // 4. ახალი TotalCopies ვერ იქნება გაცემულზე ნაკლები
            if (request.TotalCopies < borrowed)
                throw new BusinessRuleException(
                    $"TotalCopies cannot be less than currently borrowed copies ({borrowed}).");

            // 5. ველების განახლება და ხელმისაწვდომი ეგზემპლარების გადათვლა
            _mapper.Map(request, book);
            book.AvailableCopies = book.TotalCopies - borrowed;

            // 6. ჟანრების სრული ჩანაცვლება
            var genres = await _context.Genres
                .Where(g => request.GenreIds.Contains(g.Id))
                .ToListAsync();
            book.Genres.Clear();
            foreach (var genre in genres)
                book.Genres.Add(genre);

            await _context.SaveChangesAsync();

            // 7. ხელახლა ვკითხულობთ Include-ით, რომ AuthorName სწორი იყოს
            return await GetBookByIdAsync(id);
        }
    }
}
