using ApplicationCore.Models;

namespace ApplicationCore.Contracts.Services;

public interface IAdminService
{
    Task<int> CreateMovie(MovieRequestModel model);
    Task<int> UpdateMovie(MovieRequestModel model, int id);

    Task<List<MovieCardModel>> TopPurchasedMovie(DateTime? startDate = null, DateTime? endDate = null);
}