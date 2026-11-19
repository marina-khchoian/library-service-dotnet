using FluentValidation;
using LibraryService.Data;
using LibraryService.DTOs.Books;
using Microsoft.EntityFrameworkCore;
namespace LibraryService.Validators
{
    public class CreateBookRequestValidator : AbstractValidator<CreateBookRequest>
    {
        public CreateBookRequestValidator(AppDbContext db)
        {
            RuleFor(x => x.Title).NotEmpty().WithMessage("Title is required.")
                 .Length(1, 200).WithMessage("Title must be between 1 and 200 characters.");
            RuleFor(x=>x.ISBN)
                .Matches(@"^\d{10}$|^\d{13}$").WithMessage("ISBN must contain 10 or 13 digits.")
            .MustAsync(async(isbn, ct)=> !await db.Books.AnyAsync(b=> b.ISBN== isbn, ct))
            .WithMessage("A book with this ISBN already exists.");

            RuleFor(x => x.PublishedYear).InclusiveBetween(1450, DateTime.UtcNow.Year);
            RuleFor(x => x.TotalCopies).InclusiveBetween(1, 1000);
            RuleFor(x => x.AuthorId)
                .MustAsync(async(id, ct) => await db.Authors.AnyAsync(a => a.Id == id, ct))
                .WithMessage("Author does not exist.");
            RuleFor(x => x.GenreIds)
                .NotEmpty()
                .Must(ids => ids.Distinct().Count() == ids.Count).WithMessage("Duplicate genres.");
        }
    }
}
