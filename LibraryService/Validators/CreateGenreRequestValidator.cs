using FluentValidation;
using LibraryService.DTOs.Genres;
namespace LibraryService.Validators
{
    public class CreateGenreRequestValidator : AbstractValidator<CreateGenreRequest>
    {
        public CreateGenreRequestValidator()
        {
            RuleFor(x => x.Name).NotEmpty().WithMessage("Genre name is required.")
                .Length(2, 50).WithMessage("Genre name must be between 2 and 50 characters.");
        }

    }
}
