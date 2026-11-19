using AutoMapper;
using AutoMapper.QueryableExtensions;
using LibraryService.Data;
using LibraryService.DTOs.Genres;
using LibraryService.Entities;
using LibraryService.Exceptions;
using LibraryService.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace LibraryService.Services
{
    public class GenreService : IGenreService
    {
        private readonly AppDbContext context;
        private readonly IMapper mapper;

        public GenreService(AppDbContext context, IMapper mapper)
        {
            this.context = context;
            this.mapper = mapper;
        }

        public async Task<GenreDto> CreateGenreAsync(CreateGenreRequest request)
        {
            var exists = await context.Genres.AnyAsync(g => g.Name == request.Name);
            if (exists)
            {
                throw new ConflictException("Genre already exists");
            }

            var genre = mapper.Map<Genre>(request);
            context.Genres.Add(genre);
            await context.SaveChangesAsync();

            return mapper.Map<GenreDto>(genre);
        }

        public async Task<List<GenreDto>> GetAllGenreAsync()
        {
            return await context.Genres
                .ProjectTo<GenreDto>(mapper.ConfigurationProvider)
                .ToListAsync();
        }
    }
}