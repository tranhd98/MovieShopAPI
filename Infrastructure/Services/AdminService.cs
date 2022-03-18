using ApplicationCore.Contracts.Repositories;
using ApplicationCore.Contracts.Services;
using ApplicationCore.Entities;
using ApplicationCore.Models;

namespace Infrastructure.Services;

public class AdminService: IAdminService
{
    private readonly IMovieRepository _movieRepository;

    public AdminService(IMovieRepository movieRepository)
    {
        _movieRepository = movieRepository;
    }

    public async Task<int> CreateMovie(MovieRequestModel model)
    {
        var movieRequest = new Movie
        {
            Tagline = model.Tagline,
            BackdropUrl = model.BackdropUrl,
            Title = model.Title,
            Budget = model.Budget,
            TmdbUrl = model.TmdbUrl,
            RunTime = model.RunTime,
            Overview = model.Overview,
            OriginalLanguage = model.OriginalLanguage,
            PosterUrl = model.PosterUrl,
            Price = model.Price,
            Revenue = model.Revenue,
            ImdbUrl = model.ImdbUrl,
            ReleaseDate = model.ReleaseDate
        };
        var createdMovies = await _movieRepository.Add(movieRequest);
        return createdMovies.Id;
    }
    public async Task<int> UpdateMovie(MovieRequestModel model, int id)
    {
        var movie = await _movieRepository.GetById(id);
        if (movie == null)
        {
            throw new Exception("Not found");
        }
        if (!model.Title.Equals(""))
        {
            movie.Title = model.Title;
        }

        if (!model.Tagline.Equals(""))
        {
            movie.Tagline = model.Tagline;
        }

        if (model.Budget != null)
        {
            movie.Budget = model.Budget;
        }
        
        if (model.Revenue != null)
        {
            movie.Revenue = model.Revenue;
        }
        
        if (!model.ImdbUrl.Equals(""))
        {
            movie.ImdbUrl = model.ImdbUrl;
        }
        
        if (!model.TmdbUrl.Equals(""))
        {
            movie.TmdbUrl = model.TmdbUrl;
        }
        
        if (!model.PosterUrl.Equals(""))
        {
            movie.PosterUrl = model.PosterUrl;
        }

        if (!model.BackdropUrl.Equals(""))
        {
            movie.BackdropUrl = model.BackdropUrl;
        }
        
        if (!model.OriginalLanguage.Equals(""))
        {
            movie.OriginalLanguage = model.OriginalLanguage;
        }
        
        if (!model.ReleaseDate.Equals(""))
        {
            movie.ReleaseDate = model.ReleaseDate;
        }
        
        
        if (model.RunTime != null)
        {
            movie.RunTime = model.RunTime;
        }

        if (model.Price != null)
        {
            movie.Price = model.Price;
        }

        var update = await _movieRepository.Update(movie);
        return update.Id;
    }

    public async Task<List<MovieCardModel>> TopPurchasedMovie(DateTime? startDate = null, DateTime? endDate = null)
    {
        var actualEnd = endDate ?? DateTime.Now;
        var actualStart = startDate ?? actualEnd.AddDays(-90);
        var topPurchases = await _movieRepository.GetTopPurchasesMovies(actualStart, actualEnd);
        var newMovieCardModelList = new List<MovieCardModel>();

        foreach (var movie in topPurchases)
        {
            newMovieCardModelList.Add(new MovieCardModel
            {
                Id = movie.Id,
                PosterURL = movie.PosterUrl,
                Title = movie.Title
            });
        }

        return newMovieCardModelList;
    }
}