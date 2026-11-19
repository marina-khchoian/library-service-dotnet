using AutoMapper;
using LibraryService.DTOs.Books;
using LibraryService.Entities;

namespace LibraryService.Mapping
{
    public class BookProfile : Profile
    {
        public BookProfile()
        {
            // Entity -> DTO (3 custom mapping)
            CreateMap<Book, BookDto>()
                .ForMember(d => d.AuthorName,
                    o => o.MapFrom(s => s.Author.FirstName + " " + s.Author.LastName))
                .ForMember(d => d.Genres,
                    o => o.MapFrom(s => s.Genres.Select(g => g.Name)))
                .ForMember(d => d.IsAvailable,
                    o => o.MapFrom(s => s.AvailableCopies > 0));

            // Request -> Entity
            CreateMap<CreateBookRequest, Book>()
                .ForMember(d => d.Id, o => o.Ignore())
                .ForMember(d => d.AvailableCopies, o => o.MapFrom(s => s.TotalCopies))
                .ForMember(d => d.Author, o => o.Ignore())
                .ForMember(d => d.Genres, o => o.Ignore())        // სერვისში ივსება GenreIds-ით
                .ForMember(d => d.BorrowRecords, o => o.Ignore());

            CreateMap<UpdateBookRequest, Book>()
                .ForMember(d => d.Id, o => o.Ignore())
                .ForMember(d => d.AvailableCopies, o => o.Ignore()) // სერვისში ითვლება
                .ForMember(d => d.Author, o => o.Ignore())
                .ForMember(d => d.Genres, o => o.Ignore())
                .ForMember(d => d.BorrowRecords, o => o.Ignore());
        }
    }
}