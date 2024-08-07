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

            var mapperConfig = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<Author, AuthorResponse>();
                cfg.CreateMap<AuthorAddRequest, Author>();
                cfg.CreateMap<AuthorUpdateRequest, Author>();
            });
            _mapper = mapperConfig.CreateMapper();

            _authorService = new AuthorService(_authorRepositoryMock.Object, _mapper);
        }

        [Fact]
        public async Task GetAllAuthorsAsync_Should_Return_All_Authors()
        {
            // Arrange
            var authors = new List<Author>
        {
            new Author { AuthorID = Guid.NewGuid(), FirstName = "John", LastName = "Doe" },
            new Author { AuthorID = Guid.NewGuid(), FirstName = "Jane", LastName = "Doe" }
        };
            _authorRepositoryMock.Setup(repo => repo.GetAllAsync()).ReturnsAsync(authors);

            // Act
            var result = await _authorService.GetAllAuthorsAsync();

            // Assert
            Assert.NotNull(result);
            Assert.Equal(authors.Count, result.Count());
        }

        [Fact]
        public async Task GetAuthorByIdAsync_Should_Return_Author()
        {
            // Arrange
            var authorId = Guid.NewGuid();
            var author = new Author { AuthorID = authorId, FirstName = "John", LastName = "Doe" };
            _authorRepositoryMock.Setup(repo => repo.GetByIdAsync(authorId)).ReturnsAsync(author);

            // Act
            var result = await _authorService.GetAuthorByIdAsync(authorId);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(author.FirstName, result.FirstName);
            Assert.Equal(author.LastName, result.LastName);
        }

        [Fact]
        public async Task GetAuthorByNameAsync_Should_Return_Author()
        {
            // Arrange
            var firstName = "John";
            var lastName = "Doe";
            var authors = new List<Author>
        {
            new Author { AuthorID = Guid.NewGuid(), FirstName = firstName, LastName = lastName }
        };
            _authorRepositoryMock.Setup(repo => repo.GetAllAsync()).ReturnsAsync(authors);

            // Act
            var result = await _authorService.GetAuthorByNameAsync(firstName, lastName);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(firstName, result.First().FirstName);
            Assert.Equal(lastName, result.First().LastName);
        }

        [Fact]
        public async Task AddAuthorAsync_Should_Add_New_Author()
        {
            // Arrange
            var request = new AuthorAddRequest
            {
                FirstName = "John",
                LastName = "Doe",
                Biography = "A famous author.",
                DateOfBirth = new DateTime(1980, 1, 1)
            };
            var author = new Author
            {
                AuthorID = Guid.NewGuid(),
                FirstName = request.FirstName,
                LastName = request.LastName,
                Description = request.Biography,
                DateOfBirth = request.DateOfBirth.Value
            };
            _authorRepositoryMock.Setup(repo => repo.AddAsync(It.IsAny<Author>())).ReturnsAsync(true);

            // Act
            var result = await _authorService.AddAuthorAsync(request);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(request.FirstName, result.FirstName);
            Assert.Equal(request.LastName, result.LastName);
        }

        [Fact]
        public async Task DeleteAuthorAsync_Should_Delete_Author()
        {
            // Arrange
            var authorId = Guid.NewGuid();
            var author = new Author { AuthorID = authorId, Books = new List<Book>() };

            _authorRepositoryMock.Setup(repo => repo.GetByIdAsync(authorId)).ReturnsAsync(author);
            _authorRepositoryMock.Setup(repo => repo.DeleteAsync(author)).ReturnsAsync(true);

            // Act
            var result = await _authorService.DeleteAuthorAsync(authorId);

            // Assert
            Assert.True(result);
        }

        [Fact]
        public async Task DeleteAuthorAsync_Should_Return_False_When_Author_Has_Books()
        {
            // Arrange
            var authorId = Guid.NewGuid();
            var author = new Author { AuthorID = authorId, Books = new List<Book> { new Book() } };

            _authorRepositoryMock.Setup(repo => repo.GetByIdAsync(authorId)).ReturnsAsync(author);

            // Act
            var result = await _authorService.DeleteAuthorAsync(authorId);

            // Assert
            Assert.False(result);
        }
    }
}
