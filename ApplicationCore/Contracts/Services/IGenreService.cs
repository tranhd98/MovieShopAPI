using ApplicationCore.Entities;
using ApplicationCore.Models;

namespace ApplicationCore.Contracts.Services;

public interface IGenreService
{
    Task<List<GenreModel>> GetAllGenres();
}