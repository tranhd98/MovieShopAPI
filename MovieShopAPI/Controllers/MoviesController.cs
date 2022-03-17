using ApplicationCore.Contracts.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
namespace MovieShopAPI.Controllers;
// AttributeRouting
[Route("api/[controller]")]
[ApiController]
public class MoviesController : ControllerBase
{
    // in REST pattern we don't specify the http verbs in the url
    // GET
    private IMovieService _movieService;

    public MoviesController(IMovieService movieService)
    {
        _movieService = movieService;
    }
    
    [HttpGet]
    public async Task<IActionResult> Index(int pageSize = 30, int pageNumber = 1)
    {
        var pageNumbers = await _movieService.GetAllMovies(pageSize, pageNumber);
        if (pageNumbers == null)
        {
            return NotFound(new {error = "Movie Not Found"});
        }

        return Ok(pageNumbers);
    }
    
    
    // api/movies/3
    [Route("{id:int}")]
    [HttpGet]
    public async Task<IActionResult> GetMovie(int id)
    {
        var movie = await _movieService.GetMovieDetails(id);
        // return the data/json format
        // HTTP status code, 200 ok
        if (movie == null)
        {
            return NotFound(new {error = $"Movie Not Found for id: {id}"});
        }

        return Ok(movie);
    }
    
    //api/Movies/top-grossing
    [Route("top-grossing")]
    [HttpGet]
    public async Task<IActionResult> GetTopGrossingMovies()
    {
        var movies = await _movieService.GetTop30GrossingMovies();
        if (movies == null)
        {
            return NotFound(new {error = "Not found any movie"});
        }

        return Ok(movies);
    }
    [Route("top-rated")]
    [HttpGet]
    public async Task<IActionResult> Top30Ratings()
    {
        var movies = await _movieService.GetTop30RatingMovies();
        if (movies == null)
        {
            return NotFound(new {error = "not found"});
        }

        return Ok(movies);
    }

    [Route("genre/{genreId=int}")]
    [HttpGet]
    public async Task<IActionResult> GetMoviesByGenres(int genreId, int pageSize = 30, int pageNumber = 1)
    {
        var pageNumbers = await _movieService.GetMoviesByGenres(genreId, pageSize, pageNumber);
        if (pageNumbers == null)
        {
            return NotFound(new {error = "not found"});
        }

        return Ok(pageNumbers);
    }

    [Route("{id=int}/reviews")]
    [HttpGet]
    public async Task<IActionResult> GetReviewsByMovie(int id, int pageSize = 30, int pageNumber = 1)
    {
        var reviews = await _movieService.GetReviewsByMovieId(id, pageSize, pageNumber);
        if (reviews == null)
        {
            return NotFound(new {error = "not found"});
        }

        return Ok(reviews);
    }
    
}