using ApplicationCore.Contracts.Repositories;
using ApplicationCore.Contracts.Services;
using ApplicationCore.Entities;
using ApplicationCore.Models;
using Infrastructure.Repositories;

namespace Infrastructure.Services;

public class GenreService: IGenreService
{
    private readonly IGenreRepository _genreRepository;

    public GenreService(IGenreRepository genreRepository)
    {
        _genreRepository = genreRepository;
    }

    public async Task<List<GenreModel>> GetAllGenres()
    {
        var genres = await _genreRepository.GetAllGenres();
        var genresList = new List<GenreModel>();
        foreach (var g in genres)
        {
            genresList.Add(new GenreModel
            {
                Id = g.Id,
                Name = g.Name
            });
        }
        return genresList;
    }
}