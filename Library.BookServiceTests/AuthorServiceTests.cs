using AutoMapper;
using Library.Core.Automapper;
using Library.Core.Domain.Entities;
using Library.Core.Domain.RepositrotyContracts;
using Library.Core.DTO.Author;
using Library.Core.Services;
using Moq;

namespace Library.ServicesTests
{
    public class AuthorServiceTests
    {
        private readonly Mock<IAuthorRepository> _authorRepositoryMock;
        private readonly IMapper _mapper;
        private readonly AuthorService _authorService;

        public AuthorServiceTests()
        {
            _authorRepositoryMock = new Mock<IAuthorRepository>();

            var config = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<AutomapperProfile>();
            });
            _mapper = config.CreateMapper();

            _authorService = new AuthorService(_authorRepositoryMock.Object, _mapper);
        }

        [Fact]
        public async Task GetAllAuthorsAsync_ReturnsAuthors()
        {
            // Arrange
            var authors = new List<Author>
            {
                new Author { AuthorID = Guid.NewGuid(), FirstName = "John", LastName = "Doe" },
                new Author { AuthorID = Guid.NewGuid(), FirstName = "Jane", LastName = "Smith" }
            };
            _authorRepositoryMock.Setup(repo => repo.GetAllAsync()).ReturnsAsync(authors);

            // Act
            var result = await _authorService.GetAllAuthorsAsync();

            // Assert
            Assert.NotNull(result);
            Assert.Equal(authors.Count, result.Count());
        }

        [Fact]
        public async Task GetAuthorByIdAsync_ExistingId_ReturnsAuthor()
        {
            // Arrange
            var authorId = Guid.NewGuid();
            var author = new Author { AuthorID = authorId, FirstName = "John", LastName = "Doe" };
            _authorRepositoryMock.Setup(repo => repo.GetByIdAsync(authorId)).ReturnsAsync(author);

            // Act
            var result = await _authorService.GetAuthorByIdAsync(authorId);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(authorId, result.AuthorID);
        }

        [Fact]
        public async Task GetAuthorByIdAsync_NonExistingId_ReturnsNull()
        {
            // Arrange
            var authorId = Guid.NewGuid();
            _authorRepositoryMock.Setup(repo => repo.GetByIdAsync(authorId)).ReturnsAsync((Author)null);

            // Act
            var result = await _authorService.GetAuthorByIdAsync(authorId);

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public async Task AddAuthorAsync_ValidRequest_AddsAuthor()
        {
            // Arrange
            var authorAddRequest = new AuthorAddRequest
            {
                FirstName = "John",
                LastName = "Doe",
                Biography = "Biography",
                DateOfBirth = new DateTime(1990, 1, 1)
            };

            var author = new Author
            {
                AuthorID = Guid.NewGuid(),
                FirstName = "John",
                LastName = "Doe",
                Description = "Biography",
                DateOfBirth = new DateTime(1990, 1, 1)
            };

            _authorRepositoryMock.Setup(repo => repo.AddAsync(It.IsAny<Author>())).ReturnsAsync(true);

            // Act
            var result = await _authorService.AddAuthorAsync(authorAddRequest);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(authorAddRequest.FirstName, result.FirstName);
            Assert.Equal(authorAddRequest.LastName, result.LastName);
        }

        [Fact]
        public async Task UpdateAuthorAsync_ValidRequest_UpdatesAuthor()
        {
            // Arrange
            var authorUpdateRequest = new AuthorUpdateRequest
            {
                AuthorID = Guid.NewGuid(),
                FirstName = "John",
                LastName = "Doe",
                Biography = "Biography",
                DateOfBirth = new DateTime(1990, 1, 1)
            };

            var author = new Author
            {
                AuthorID = authorUpdateRequest.AuthorID,
                FirstName = "OldFirstName",
                LastName = "OldLastName",
                Description = "OldBiography",
                DateOfBirth = new DateTime(1980, 1, 1)
            };

            _authorRepositoryMock.Setup(repo => repo.GetByIdAsync(author.AuthorID)).ReturnsAsync(author);
            _authorRepositoryMock.Setup(repo => repo.UpdateAsync(It.IsAny<Author>())).ReturnsAsync(true);

            // Act
            var result = await _authorService.UpdateAuthorAsync(authorUpdateRequest);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(authorUpdateRequest.FirstName, result.FirstName);
            Assert.Equal(authorUpdateRequest.LastName, result.LastName);
        }

        [Fact]
        public async Task DeleteAuthorAsync_ExistingId_DeletesAuthor()
        {
            // Arrange
            var authorId = Guid.NewGuid();
            _authorRepositoryMock.Setup(repo => repo.DeleteAsync(authorId)).ReturnsAsync(true);

            // Act
            var result = await _authorService.DeleteAuthorAsync(authorId);

            // Assert
            Assert.True(result);
        }

        [Fact]
        public async Task DeleteAuthorAsync_NonExistingId_ThrowsException()
        {
            // Arrange
            var authorId = Guid.NewGuid();
            _authorRepositoryMock.Setup(repo => repo.DeleteAsync(authorId)).ReturnsAsync(false);

            // Act & Assert
            await Assert.ThrowsAsync<InvalidOperationException>(() => _authorService.DeleteAuthorAsync(authorId));
        }
    }
}
