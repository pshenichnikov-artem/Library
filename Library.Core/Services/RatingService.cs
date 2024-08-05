using AutoMapper;
using Library.Core.Domain.Entities;
using Library.Core.Domain.RepositrotyContracts;
using Library.Core.DTO.Rating;
using Library.Core.ServiceContracts;

namespace Library.Core.Services
{
    public class RatingService : IRatingService
    {
        private readonly IRatingRepository _ratingRepository;
        private readonly IMapper _mapper;

        public RatingService(IRatingRepository ratingRepository, IMapper mapper)
        {
            _ratingRepository = ratingRepository;
            _mapper = mapper;
        }

        public async Task<IEnumerable<RatingResponse>> GetAllRatingsAsync()
        {
            var ratings = await _ratingRepository.GetAllAsync();
            return _mapper.Map<IEnumerable<RatingResponse>>(ratings);
        }

        public async Task<RatingResponse> AddRatingAsync(RatingRequest? ratingRequest)
        {
            if (ratingRequest == null)
            {
                throw new ArgumentNullException(nameof(ratingRequest), "RatingRequest cannot be null.");
            }

            var rating = _mapper.Map<Rating>(ratingRequest);
            rating.RatingID = Guid.NewGuid();

            var success = await _ratingRepository.AddAsync(rating);
            if (!success)
            {
                throw new InvalidOperationException("Failed to add the rating.");
            }

            return _mapper.Map<RatingResponse>(rating);
        }

        public async Task<RatingResponse?> UpdateRatingAsync(RatingRequest? ratingRequest)
        {
            if (ratingRequest == null)
            {
                throw new ArgumentNullException(nameof(ratingRequest), "RatingRequest cannot be null.");
            }

            var existingRating = await _ratingRepository.GetByUserIdAndBookIdAsync(ratingRequest.UserID.Value, ratingRequest.BookID.Value);
            if (existingRating == null)
            {
                return null;
            }

            existingRating.Value = ratingRequest.Value.Value;

            var success = await _ratingRepository.UpdateAsync(existingRating);
            if (!success)
            {
                throw new InvalidOperationException("Failed to update the rating.");
            }

            return _mapper.Map<RatingResponse>(existingRating);
        }

        public async Task<bool> DeleteRatingAsync(Guid? userId, Guid? bookId)
        {
            if (userId == null || bookId == null)
            {
                throw new ArgumentNullException(nameof(userId) + nameof(bookId), "Rating ID cannot be null.");
            }

            var rating = await _ratingRepository.GetByUserIdAndBookIdAsync(userId.Value, bookId.Value);
            if (rating == null)
            {
                return false;
            }

            return await _ratingRepository.DeleteAsync(rating);
        }

        public async Task<IEnumerable<RatingResponse>> GetRatingsByUserIdAsync(Guid? userId)
        {
            if (userId == null)
            {
                throw new ArgumentNullException(nameof(userId), "User ID cannot be null.");
            }

            var ratings = await _ratingRepository.GetByUserIdAsync(userId.Value);
            return _mapper.Map<IEnumerable<RatingResponse>>(ratings);
        }

        public async Task<IEnumerable<RatingResponse>> GetRatingsByBookIdAsync(Guid? bookId)
        {
            if (bookId == null)
            {
                throw new ArgumentNullException(nameof(bookId), "Book ID cannot be null.");
            }

            var ratings = await _ratingRepository.GetByBookIdAsync(bookId.GetValueOrDefault());
            return _mapper.Map<IEnumerable<RatingResponse>>(ratings);
        }

        public async Task<RatingResponse?> GetRatingByUserIdAndBookIdAsync(Guid? userId, Guid? bookId)
        {
            if (userId == null || bookId == null)
            {
                throw new ArgumentNullException("User ID and Book ID cannot be null.");
            }

            var rating = await _ratingRepository.GetByUserIdAndBookIdAsync(userId.GetValueOrDefault(), bookId.GetValueOrDefault());
            if (rating == null)
                return null;

            return _mapper.Map<RatingResponse>(rating);
        }
    }
}
