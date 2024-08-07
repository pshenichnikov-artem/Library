using AutoMapper;
using Library.Core.Domain.Entities;
using Library.Core.Domain.RepositrotyContracts;
using Library.Core.DTO.Rating;
using Library.Core.Services;
using Moq;

namespace Library.ServicesTests
{
    public class RatingServiceTests
    {
        private readonly Mock<IRatingRepository> _ratingRepositoryMock;
        private readonly Mock<IMapper> _mapperMock;
        private readonly RatingService _ratingService;

        public RatingServiceTests()
        {
            _ratingRepositoryMock = new Mock<IRatingRepository>();
            _mapperMock = new Mock<IMapper>();

            _ratingService = new RatingService(
                _ratingRepositoryMock.Object,
                _mapperMock.Object
            );
        }

        [Fact]
        public async Task GetAllRatingsAsync_Should_Return_Ratings()
        {
            // Arrange
            var ratings = new List<Rating>
        {
            new Rating { RatingID = Guid.NewGuid(), BookID = Guid.NewGuid(), UserID = Guid.NewGuid(), Value = 4.5f },
            new Rating { RatingID = Guid.NewGuid(), BookID = Guid.NewGuid(), UserID = Guid.NewGuid(), Value = 3.5f }
        };

            var ratingResponses = new List<RatingResponse>
        {
            new RatingResponse { BookID = ratings[0].BookID, Value = 4.5f, UsersRaiting = new Dictionary<Guid, float> { { ratings[0].UserID, 4.5f } } },
            new RatingResponse { BookID = ratings[1].BookID, Value = 3.5f, UsersRaiting = new Dictionary<Guid, float> { { ratings[1].UserID, 3.5f } } }
        };

            _ratingRepositoryMock.Setup(repo => repo.GetAllAsync())
                .ReturnsAsync(ratings);

            _mapperMock.Setup(mapper => mapper.Map<IEnumerable<RatingResponse>>(ratings))
                .Returns(ratingResponses);

            // Act
            var result = await _ratingService.GetAllRatingsAsync();

            // Assert
            Assert.NotNull(result);
            Assert.Equal(ratingResponses.Count, result.Count());
            Assert.Contains(result, r => r.Value == 4.5f);
            Assert.Contains(result, r => r.Value == 3.5f);
        }

        [Fact]
        public async Task AddRatingAsync_Should_Add_New_Rating()
        {
            // Arrange
            var ratingRequest = new RatingRequest
            {
                BookID = Guid.NewGuid(),
                UserID = Guid.NewGuid(),
                Value = 5.0f
            };

            var rating = new Rating
            {
                RatingID = Guid.NewGuid(),
                BookID = ratingRequest.BookID.Value,
                UserID = ratingRequest.UserID.Value,
                Value = ratingRequest.Value.Value
            };

            var ratingResponse = new RatingResponse
            {
                BookID = rating.BookID,
                Value = rating.Value,
                UsersRaiting = new Dictionary<Guid, float> { { rating.UserID, rating.Value } }
            };

            _mapperMock.Setup(mapper => mapper.Map<Rating>(ratingRequest))
                .Returns(rating);

            _mapperMock.Setup(mapper => mapper.Map<RatingResponse>(rating))
                .Returns(ratingResponse);

            _ratingRepositoryMock.Setup(repo => repo.AddAsync(rating))
                .ReturnsAsync(true);

            // Act
            var result = await _ratingService.AddRatingAsync(ratingRequest);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(ratingResponse.Value, result.Value);
            Assert.Contains(result.UsersRaiting, u => u.Key == rating.UserID && u.Value == rating.Value);
        }

        [Fact]
        public async Task DeleteRatingAsync_Should_Delete_Rating()
        {
            // Arrange
            var userId = Guid.NewGuid();
            var bookId = Guid.NewGuid();
            var rating = new Rating
            {
                RatingID = Guid.NewGuid(),
                BookID = bookId,
                UserID = userId,
                Value = 4.5f
            };

            _ratingRepositoryMock.Setup(repo => repo.GetByUserIdAndBookIdAsync(userId, bookId))
                .ReturnsAsync(rating);

            _ratingRepositoryMock.Setup(repo => repo.DeleteAsync(rating))
                .ReturnsAsync(true);

            // Act
            var result = await _ratingService.DeleteRatingAsync(userId, bookId);

            // Assert
            Assert.True(result);
        }

        [Fact]
        public async Task GetRatingsByUserIdAsync_Should_Return_Ratings()
        {
            // Arrange
            var userId = Guid.NewGuid();
            var ratings = new List<Rating>
        {
            new Rating { RatingID = Guid.NewGuid(), BookID = Guid.NewGuid(), UserID = userId, Value = 4.0f },
            new Rating { RatingID = Guid.NewGuid(), BookID = Guid.NewGuid(), UserID = userId, Value = 5.0f }
        };

            var ratingResponses = new List<RatingResponse>
        {
            new RatingResponse { BookID = ratings[0].BookID, Value = 4.0f, UsersRaiting = new Dictionary<Guid, float> { { userId, 4.0f } } },
            new RatingResponse { BookID = ratings[1].BookID, Value = 5.0f, UsersRaiting = new Dictionary<Guid, float> { { userId, 5.0f } } }
        };

            _ratingRepositoryMock.Setup(repo => repo.GetByUserIdAsync(userId))
                .ReturnsAsync(ratings);

            _mapperMock.Setup(mapper => mapper.Map<IEnumerable<RatingResponse>>(ratings))
                .Returns(ratingResponses);

            // Act
            var result = await _ratingService.GetRatingsByUserIdAsync(userId);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(ratingResponses.Count, result.Count());
            Assert.Contains(result, r => r.Value == 4.0f);
            Assert.Contains(result, r => r.Value == 5.0f);
        }

        [Fact]
        public async Task GetRatingsByBookIdAsync_Should_Return_Ratings()
        {
            // Arrange
            var bookId = Guid.NewGuid();
            var ratings = new List<Rating>
        {
            new Rating { RatingID = Guid.NewGuid(), BookID = bookId, UserID = Guid.NewGuid(), Value = 3.5f },
            new Rating { RatingID = Guid.NewGuid(), BookID = bookId, UserID = Guid.NewGuid(), Value = 4.0f }
        };

            var ratingResponses = new List<RatingResponse>
        {
            new RatingResponse { BookID = bookId, Value = 3.5f, UsersRaiting = new Dictionary<Guid, float> { { ratings[0].UserID, 3.5f } } },
            new RatingResponse { BookID = bookId, Value = 4.0f, UsersRaiting = new Dictionary<Guid, float> { { ratings[1].UserID, 4.0f } } }
        };

            _ratingRepositoryMock.Setup(repo => repo.GetByBookIdAsync(bookId))
                .ReturnsAsync(ratings);

            _mapperMock.Setup(mapper => mapper.Map<IEnumerable<RatingResponse>>(ratings))
                .Returns(ratingResponses);

            // Act
            var result = await _ratingService.GetRatingsByBookIdAsync(bookId);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(ratingResponses.Count, result.Count());
            Assert.Contains(result, r => r.Value == 3.5f);
            Assert.Contains(result, r => r.Value == 4.0f);
        }

        [Fact]
        public async Task GetRatingByUserIdAndBookIdAsync_Should_Return_Rating()
        {
            // Arrange
            var userId = Guid.NewGuid();
            var bookId = Guid.NewGuid();
            var rating = new Rating
            {
                RatingID = Guid.NewGuid(),
                BookID = bookId,
                UserID = userId,
                Value = 4.0f
            };

            var ratingResponse = new RatingResponse
            {
                BookID = rating.BookID,
                Value = rating.Value,
                UsersRaiting = new Dictionary<Guid, float> { { rating.UserID, rating.Value } }
            };

            _ratingRepositoryMock.Setup(repo => repo.GetByUserIdAndBookIdAsync(userId, bookId))
                .ReturnsAsync(rating);

            _mapperMock.Setup(mapper => mapper.Map<RatingResponse>(rating))
                .Returns(ratingResponse);

            // Act
            var result = await _ratingService.GetRatingByUserIdAndBookIdAsync(userId, bookId);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(ratingResponse.Value, result.Value);
            Assert.Contains(result.UsersRaiting, u => u.Key == rating.UserID && u.Value == rating.Value);
        }
    }
}
