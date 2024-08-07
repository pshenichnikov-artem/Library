using AutoMapper;
using Library.Core.Domain.Entities;
using Library.Core.Domain.RepositrotyContracts;
using Library.Core.DTO.Comment;
using Library.Core.Services;
using Moq;

namespace Library.ServicesTests
{
    public class CommentServiceTests
    {
        private readonly Mock<ICommentRepository> _mockCommentRepository;
        private readonly Mock<IMapper> _mockMapper;
        private readonly CommentService _commentService;

        public CommentServiceTests()
        {
            _mockCommentRepository = new Mock<ICommentRepository>();
            _mockMapper = new Mock<IMapper>();
            _commentService = new CommentService(_mockCommentRepository.Object, _mockMapper.Object);
        }

        [Fact]
        public async Task AddCommentAsync_ValidRequest_ShouldReturnCommentResponse()
        {
            // Arrange
            var request = new CommentAddRequest
            {
                BookID = Guid.NewGuid(),
                UserID = Guid.NewGuid(),
                Content = "This is a comment"
            };
            var comment = new Comment
            {
                CommentID = Guid.NewGuid(),
                BookID = request.BookID,
                UserID = request.UserID,
                Content = request.Content,
                CreatedAt = DateTime.Now
            };

            _mockMapper.Setup(m => m.Map<Comment>(request)).Returns(comment);
            _mockMapper.Setup(m => m.Map<CommentResponse>(It.IsAny<Comment>())).Returns(new CommentResponse());
            _mockCommentRepository.Setup(r => r.AddAsync(It.IsAny<Comment>())).ReturnsAsync(true);

            // Act
            var result = await _commentService.AddCommentAsync(request);

            // Assert
            Assert.NotNull(result);
            _mockMapper.Verify(m => m.Map<Comment>(request), Times.Once);
            _mockMapper.Verify(m => m.Map<CommentResponse>(It.IsAny<Comment>()), Times.Once);
            _mockCommentRepository.Verify(r => r.AddAsync(It.IsAny<Comment>()), Times.Once);
        }

        [Fact]
        public async Task GetCommentByIdAsync_ValidId_ShouldReturnCommentResponse()
        {
            // Arrange
            var commentId = Guid.NewGuid();
            var comment = new Comment
            {
                CommentID = commentId,
                BookID = Guid.NewGuid(),
                UserID = Guid.NewGuid(),
                Content = "This is a comment",
                CreatedAt = DateTime.Now
            };

            _mockCommentRepository.Setup(r => r.GetByIdAsync(commentId)).ReturnsAsync(comment);
            _mockMapper.Setup(m => m.Map<CommentResponse>(comment)).Returns(new CommentResponse());

            // Act
            var result = await _commentService.GetCommentByIdAsync(commentId);

            // Assert
            Assert.NotNull(result);
            _mockMapper.Verify(m => m.Map<CommentResponse>(comment), Times.Once);
            _mockCommentRepository.Verify(r => r.GetByIdAsync(commentId), Times.Once);
        }

        [Fact]
        public async Task GetCommentsByBookIdAsync_ValidBookId_ShouldReturnComments()
        {
            // Arrange
            var bookId = Guid.NewGuid();
            var comments = new List<Comment>
        {
            new Comment { BookID = bookId, UserID = Guid.NewGuid(), Content = "Comment 1" },
            new Comment { BookID = bookId, UserID = Guid.NewGuid(), Content = "Comment 2" }
        };

            _mockCommentRepository.Setup(r => r.GetAllAsync()).ReturnsAsync(comments);
            _mockMapper.Setup(m => m.Map<IEnumerable<CommentResponse>>(comments)).Returns(new List<CommentResponse>());

            // Act
            var result = await _commentService.GetCommentsByBookIdAsync(bookId);

            // Assert
            Assert.NotNull(result);
            _mockMapper.Verify(m => m.Map<IEnumerable<CommentResponse>>(comments), Times.Once);
            _mockCommentRepository.Verify(r => r.GetAllAsync(), Times.Once);
        }

        [Fact]
        public async Task GetCommentsByUserIdAsync_ValidUserId_ShouldReturnComments()
        {
            // Arrange
            var userId = Guid.NewGuid();
            var comments = new List<Comment>
        {
            new Comment { BookID = Guid.NewGuid(), UserID = userId, Content = "Comment 1" },
            new Comment { BookID = Guid.NewGuid(), UserID = userId, Content = "Comment 2" }
        };

            _mockCommentRepository.Setup(r => r.GetAllAsync()).ReturnsAsync(comments);
            _mockMapper.Setup(m => m.Map<IEnumerable<CommentResponse>>(comments)).Returns(new List<CommentResponse>());

            // Act
            var result = await _commentService.GetCommentsByUserIdAsync(userId);

            // Assert
            Assert.NotNull(result);
            _mockMapper.Verify(m => m.Map<IEnumerable<CommentResponse>>(comments), Times.Once);
            _mockCommentRepository.Verify(r => r.GetAllAsync(), Times.Once);
        }

        [Fact]
        public async Task UpdateCommentAsync_ValidRequest_ShouldReturnTrue()
        {
            // Arrange
            var commentId = Guid.NewGuid();
            var request = new CommentUpdateRequest { Content = "Updated content" };
            var existingComment = new Comment
            {
                CommentID = commentId,
                BookID = Guid.NewGuid(),
                UserID = Guid.NewGuid(),
                Content = "Old content",
                CreatedAt = DateTime.Now
            };

            _mockCommentRepository.Setup(r => r.GetByIdAsync(commentId)).ReturnsAsync(existingComment);
            _mockMapper.Setup(m => m.Map(request, existingComment)).Verifiable();
            _mockCommentRepository.Setup(r => r.UpdateAsync(existingComment)).ReturnsAsync(true);

            // Act
            var result = await _commentService.UpdateCommentAsync(commentId, request);

            // Assert
            Assert.True(result);
            _mockMapper.Verify(m => m.Map(request, existingComment), Times.Once);
            _mockCommentRepository.Verify(r => r.UpdateAsync(existingComment), Times.Once);
        }
    }
}