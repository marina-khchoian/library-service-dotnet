using FluentValidation;
using LibraryService.DTOs.Authors;
namespace LibraryService.Validators
{
    public class UpdateAuthorRequestValidator : AbstractValidator<UpdateAuthorRequest>
    {
        public UpdateAuthorRequestValidator()
        {
            RuleFor(x => x.FirstName).NotEmpty().WithMessage("First name is required.")
                .Length(2, 50).WithMessage("First name must be between 2 and 50 characters.");
            RuleFor(x => x.LastName).NotEmpty().WithMessage("Last name is required.")
                .Length(2, 50).WithMessage("Last name must be between 2 and 50 characters.");
            RuleFor(x => x.BirthYear).InclusiveBetween(1000, DateTime.Now.Year)
                .WithMessage($"Birth year must be between 1000 and {DateTime.Now.Year}.");
            RuleFor(x => x.Country).NotEmpty().WithMessage("Country is required.")
                .Length(2, 50).WithMessage("Country must be between 2 and 50 characters.");
        }
    }
}
