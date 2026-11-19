using LibraryService.Data;
using FluentValidation;
using LibraryService.DTOs.Books;
using Microsoft.EntityFrameworkCore;

namespace LibraryService.Validators
{
    public class UpdateBookRequestValidator : AbstractValidator<UpdateBookRequest>
    {
        public UpdateBookRequestValidator(AppDbContext db)
        {
            RuleFor(x => x.Title).NotEmpty().WithMessage("Title is required.")
                 .Length(1, 200).WithMessage("Title must be between 1 and 200 characters.");
            RuleFor(x => x.PublishedYear).InclusiveBetween(1450, DateTime.UtcNow.Year);
            RuleFor(x => x.TotalCopies).InclusiveBetween(1, 1000);
            RuleFor(x => x.AuthorId)
                .MustAsync(async (id, ct) => await db.Authors.AnyAsync(a => a.Id == id, ct))
                .WithMessage("Author does not exist.");
            RuleFor(x => x.GenreIds)
                .NotEmpty()
                .Must(ids => ids.Distinct().Count() == ids.Count).WithMessage("Duplicate genres.");
        }
    }
}
