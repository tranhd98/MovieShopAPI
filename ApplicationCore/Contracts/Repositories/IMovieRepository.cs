using ApplicationCore.Entities;
using ApplicationCore.Models;

namespace ApplicationCore.Contracts.Repositories;

public interface IMovieRepository: IRespository<Movie>
{
    Task<IEnumerable<Movie>> GetTop30RevenueMovies();

    Task<PagedResultSet<Movie>> GetMoviesByGenres(int genreId, int pageSize = 30, int pageNumber = 1);
    Task<PagedResultSet<Movie>> GetAllMovies(int pageSize = 30, int pageNumber = 1);
    Task<IEnumerable<Movie>> GetTop30RatingMovies();

    Task<IEnumerable<Review>> GetReviews(int id, int pageSize = 30, int pageNumber = 1);
}