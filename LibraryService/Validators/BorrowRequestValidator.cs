using FluentValidation;
using LibraryService.Data;
using LibraryService.DTOs.Borrows;
using Microsoft.EntityFrameworkCore;

namespace LibraryService.Validators
{
    public class BorrowRequestValidator : AbstractValidator<BorrowRequest>
    {
        public BorrowRequestValidator(AppDbContext db)
        {
            RuleFor(x => x.BookId)
                .GreaterThan(0).WithMessage("BookId must be a positive number.")
                .MustAsync(async (id, ct) => await db.Books.AnyAsync(b => b.Id == id, ct))
                .WithMessage("Book does not exist.");
        }
    }
}