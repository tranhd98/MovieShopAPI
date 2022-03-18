using ApplicationCore.Contracts.Repositories;
using ApplicationCore.Entities;
using ApplicationCore.Models;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;

public class MovieRepository: EfRepository<Movie>, IMovieRepository
{
    public MovieRepository(MovieShopDbContext dbContext) : base(dbContext)
    {
    }
    public async Task< IEnumerable<Movie> >GetTop30RevenueMovies()
    {
        var movies = await _dbContext.Movie.OrderByDescending(m => m.Revenue).Take(30).ToListAsync();
        return movies; 
    }

    public async Task<PagedResultSet<Movie>> GetMoviesByGenres(int genreId, int pageSize = 30, int pageNumber = 1)
    {
        var count = await _dbContext.MovieGenres.Where(mg => mg.GenreId == genreId).CountAsync();
        if (count == 0)
        {
            throw new Exception("None movies existed");
        }

        var movies = await _dbContext.MovieGenres
            .Where(mg => mg.GenreId == genreId)
            .Include(mg => mg.Movie)
            .OrderBy(mg => mg.MovieId)
            .Select(mg => new Movie
            {
                Id = mg.MovieId,
                PosterUrl = mg.Movie.PosterUrl,
                Title = mg.Movie.Title
            })
            .Skip((pageNumber - 1) * pageSize).Take(pageSize).ToListAsync();
        var pagedMovies = new PagedResultSet<Movie>(movies, pageNumber, pageSize, count);
        return pagedMovies;
    }

    public async Task<PagedResultSet<Movie>> GetAllMovies(int pageSize = 30, int pageNumber = 1)
    {
        var count = await _dbContext.Movie.CountAsync();
        if (count == 0)
        {
            throw new Exception("None movies existed");
        }

        var movies = await _dbContext.Movie
            .OrderBy(m => m.Id)
            .Skip((pageNumber - 1) * pageSize).Take(pageSize).ToListAsync();
        var pagedMovies = new PagedResultSet<Movie>(movies, pageNumber, pageSize, count);
        return pagedMovies;
    }
    
    

    public async Task<IEnumerable<Movie>> GetTop30RatingMovies()
    {
        var reviewsRating = await _dbContext.Reviews.GroupBy(t => new {Id = t.MovieId})
            .Select(g => new
            {
                Average = g.Average(r => r.Rating),
                ID = g.Key.Id
            }).OrderByDescending(g=> g.Average).Take(30).ToListAsync();
        var movies = new List<Movie>();
        foreach (var v in reviewsRating)
        {
            movies.Add(await GetById(v.ID));
        }
        return movies;
    }

    public async Task<IEnumerable<Movie>> GetTopPurchasesMovies(DateTime startDate, DateTime endDate)
    {
        var purchase = await _dbContext.Purchases
            .Where(p => p.PurchaseDateTime >= startDate && p.PurchaseDateTime <= endDate)
            .GroupBy(p => new {Id = p.MovieId})
            .Select(g => new
            {
                Count = g.Count(),
                ID = g.Key.Id
            })
            .OrderByDescending(g => g.Count).ToListAsync();
        var movies = new List<Movie>();
        foreach (var p in purchase)
        {
            movies.Add(await GetById(p.ID));
        }

        return movies;
    }

    public async Task<IEnumerable<Review>> GetReviews(int id, int pageSize = 30, int pageNumber = 1)
    {
        var reviews = await _dbContext.Reviews.Where(r => r.MovieId == id)
            .OrderBy(m => m.Rating)
            .Skip((pageNumber - 1) * pageSize).Take(pageSize).ToListAsync();
        return reviews;
    }

    public async override Task<Movie> GetById(int id)
    {
        var movieDetails = await _dbContext.Movie
            .Include(m => m.Genres).ThenInclude(m => m.Genre)
            .Include(m => m.Trailers)
            .Include(m => m.MovieCasts).ThenInclude(m => m.Cast)
            .Include(m=> m.Reviews)
            .FirstOrDefaultAsync(m => m.Id == id);
        return movieDetails;
    }
    
    
}