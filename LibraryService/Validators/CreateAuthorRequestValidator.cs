using LibraryService.DTOs.Authors;
using FluentValidation;
using System.Data;
using LibraryService.Data;

namespace LibraryService.Validators
{
    public class CreateAuthorRequestValidator : AbstractValidator<CreateAuthorRequest>
    {
        public CreateAuthorRequestValidator()
        {
            RuleFor(x => x.FirstName).NotEmpty().WithMessage("First name is required.")
                .Length(1, 60).WithMessage("First name must be between 1 and 100 characters.");
            RuleFor(x => x.LastName).NotEmpty().WithMessage("Last name is required.")
                .Length(1, 60).WithMessage("Last name must be between 1 and 100 characters.");
            RuleFor(x => x.BirthYear).InclusiveBetween(1000, DateTime.UtcNow.Year)
                .WithMessage($"Birth year must be between 1000 and {DateTime.UtcNow.Year}.");
            RuleFor(x => x.Country).NotEmpty().WithMessage("Country is required.")
                .Length(2, 60).WithMessage("Country must be between 2 and 100 characters.");
        }

    }
}