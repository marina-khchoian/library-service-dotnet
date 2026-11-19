using AutoMapper;
using LibraryService.DTOs.Authors;
using LibraryService.DTOs.Books;
using LibraryService.Entities;

namespace LibraryService.Mapping
{
    public class AuthorProfile : Profile
    {
        public AuthorProfile()
        {
            // Entity -> სიის DTO: ველების სახელები ერთნაირია, ამიტომ ავტომატურად
            CreateMap<Author, AuthorDto>();

            // Entity -> დეტალების DTO: BooksCount გამოთვლადი ველია (custom mapping)
            // Books ავტომატურად აიღება, რადგან ქვემოთ Book -> BookShortDto გვაქვს
            CreateMap<Author, AuthorDetailsDto>()
                .ForMember(d => d.BooksCount, o => o.MapFrom(s => s.Books.Count));

            // წიგნი მოკლე ფორმით (Id, Title, PublishedYear)
            CreateMap<Book, BookShortDto>();

            // Request -> Entity: Id ბაზა ქმნის, Books ცალკე კავშირია
            CreateMap<CreateAuthorRequest, Author>()
                .ForMember(d => d.Id, o => o.Ignore())
                .ForMember(d => d.Books, o => o.Ignore());

            CreateMap<UpdateAuthorRequest, Author>()
                .ForMember(d => d.Id, o => o.Ignore())
                .ForMember(d => d.Books, o => o.Ignore());
        }
    }
}