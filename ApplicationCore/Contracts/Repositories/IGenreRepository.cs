using ApplicationCore.Entities;

namespace ApplicationCore.Contracts.Repositories;

public interface IGenreRepository: IRespository<Genre>
{
    Task<IEnumerable<Genre>> GetAllGenres();
}