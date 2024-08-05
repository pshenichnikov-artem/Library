using Library.Core.Domain.Entities;
using Library.Core.DTO.Rating;

namespace Library.Core.ServiceContracts
{
    public interface IRatingService
    {
        Task<IEnumerable<RatingResponse>> GetAllRatingsAsync();
        Task<RatingResponse> AddRatingAsync(RatingRequest? ratingRequest);
        Task<RatingResponse?> UpdateRatingAsync(RatingRequest? ratingRequest);
        Task<bool> DeleteRatingAsync(Guid? userId, Guid? bookId);
        Task<IEnumerable<RatingResponse>> GetRatingsByUserIdAsync(Guid? userId);
        Task<IEnumerable<RatingResponse>> GetRatingsByBookIdAsync(Guid? bookId);
        Task<RatingResponse?> GetRatingByUserIdAndBookIdAsync(Guid? userId, Guid? bookId);
    }
}
