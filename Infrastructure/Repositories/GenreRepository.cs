using ApplicationCore.Contracts.Repositories;
using ApplicationCore.Entities;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;

public class GenreRepository: EfRepository<Genre>, IGenreRepository
{
    public GenreRepository(MovieShopDbContext dbContext) : base(dbContext)
    {
    }

    public async Task<IEnumerable<Genre>> GetAllGenres()
    {
        var genres = await _dbContext.Genres.ToListAsync();
        return genres;
    }
    
}