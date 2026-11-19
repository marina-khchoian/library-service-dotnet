using AutoMapper;
using LibraryService.DTOs.LibraryCards;
using LibraryService.Entities;

namespace LibraryService.Mapping
{
    public class LibraryCardProfile : Profile
    {
        public LibraryCardProfile()
        {
            CreateMap<LibraryCard, LibraryCardDto>()
                .ForMember(d => d.OwnerName, o => o.MapFrom(s => s.User.FullName))
                .ForMember(d => d.IsActive, o => o.MapFrom(s => s.ExpiresAt > DateTime.UtcNow));
        }
    }
}