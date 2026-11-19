using AutoMapper;
using LibraryService.DTOs.Genres;
using LibraryService.Entities;

namespace LibraryService.Mapping
{
    public class GenreProfile : Profile
    {
        public GenreProfile()
        {
            CreateMap<Genre, GenreDto>();

            CreateMap<CreateGenreRequest, Genre>()
                .ForMember(d => d.Id, o => o.Ignore())
                .ForMember(d => d.Books, o => o.Ignore());
        }
    }
}