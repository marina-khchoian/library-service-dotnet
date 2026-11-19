using AutoMapper;
using LibraryService.DTOs.Borrows;
using LibraryService.Services;
using LibraryService.Entities;

namespace LibraryService.Mapping
{
    public class BorrowProfile : Profile
    {
        public BorrowProfile()
        {
            CreateMap<BorrowRecord, BorrowDto>()
                .ForMember(d => d.BookTitle, o => o.MapFrom(s => s.Book.Title))
                .ForMember(d => d.ReaderName, o => o.MapFrom(s => s.User.FullName))
                // ვადაგადაცილებულია: ჯერ არ დაუბრუნებია და DueDate გავიდა. ბაზაში არ ინახება
                .ForMember(d => d.IsOverdue,
                    o => o.MapFrom(s => s.ReturnedAt == null && s.DueDate < DateTime.UtcNow))
                // რამდენი სრული დღით არის გადაცილებული; 0, თუ ვადა არ გასულა
                .ForMember(d => d.DaysOverdue,
                    o => o.MapFrom(s => s.ReturnedAt == null && s.DueDate < DateTime.UtcNow
                        ? (int)(DateTime.UtcNow - s.DueDate).TotalDays
                        : 0))
            .ForMember(d => d.FineAmount, o => o.MapFrom(s => FineCalculator.Fine(s)))
.ForMember(d => d.FinePaid, o => o.MapFrom(s => s.FinePaidAt != null));
        }
    }
}