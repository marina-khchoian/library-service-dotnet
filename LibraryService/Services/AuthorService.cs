using AutoMapper;
using LibraryService.Data;
using LibraryService.DTOs.Authors;
using LibraryService.DTOs.Common;
using LibraryService.Exceptions;
using Microsoft.EntityFrameworkCore;
using LibraryService.Services.Interfaces;
namespace LibraryService.Services
{
    public class AuthorService : IAuthorService
    {
        private readonly IMapper mapper;
        private readonly AppDbContext context;
        public AuthorService(IMapper mapper, AppDbContext context)
        {
            this.mapper = mapper;
            this.context = context;
        }
        public async Task<PagedResult<AuthorDto>> GetAllAsync(AuthorQueryParameters query)
        {
            var page = query.Page < 1 ? 1 : query.Page;
            var pageSize = query.PageSize < 1 ? 20 : Math.Min(query.PageSize, 100);
            var authorsQuery = context.Authors.AsQueryable();
            if (!string.IsNullOrWhiteSpace(query.Search))
                authorsQuery = authorsQuery.Where(a =>
                    a.FirstName.Contains(query.Search) || a.LastName.Contains(query.Search));
            var totalCount = await authorsQuery.CountAsync();
            var authors = await authorsQuery
                .OrderBy(a => a.LastName).ThenBy(a => a.FirstName)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();
            var authorDtos = mapper.Map<List<AuthorDto>>(authors);
            return new PagedResult<AuthorDto>
            {
                TotalCount = totalCount,
                Items = authorDtos,
                Page = page,
                PageSize = pageSize
            };
        }
        public async Task<AuthorDetailsDto> GetByIdAsync(int id)
        {
            var author = await context.Authors
                .Include(a => a.Books)
                .FirstOrDefaultAsync(a => a.Id == id);
            if (author == null)
                throw new NotFoundException("Author not found");
            var authorDetailsDto = mapper.Map<AuthorDetailsDto>(author);
            return authorDetailsDto;
        }
        public async Task<AuthorDto> CreateAsync(CreateAuthorRequest request)
        {
            var author = mapper.Map<Entities.Author>(request);
            context.Authors.Add(author);
            await context.SaveChangesAsync();
            return mapper.Map<AuthorDto>(author);
        }
        public async Task<AuthorDto> UpdateAsync(int id, UpdateAuthorRequest request)
        {
            var author = await context.Authors.FindAsync(id);
            if (author == null)
                throw new NotFoundException("Author not found");
            mapper.Map(request, author);
            await context.SaveChangesAsync();
            return mapper.Map<AuthorDto>(author);
        }
        public async Task DeleteAsync(int id)
        {
            var author = await context.Authors.FindAsync(id);
            if (author == null)
                throw new NotFoundException("Author not found");
            context.Authors.Remove(author);
            await context.SaveChangesAsync();
        }

    }
}
