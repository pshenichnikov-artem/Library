using AutoMapper;
using Library.Core.Automapper;
using Library.Core.Domain.Entities;
using Library.Core.Domain.RepositrotyContracts;
using Library.Core.DTO;
using Library.Core.DTO.Book;
using Library.Core.DTO.Rating;
using Library.Core.Enums;
using Library.Core.Services;
using Moq;

namespace Library.ServiceTests
{
    public class BookServiceTests
    {
        private readonly Mock<IBookRepository> _bookRepositoryMock;
        private readonly Mock<IBookImageRepository> _bookImageRepositoryMock;
        private readonly Mock<IBookFileRepository> _bookFileRepositoryMock;
        private readonly Mock<IBookAuthorRepository> _bookAuthorRepositoryMock;
        private readonly Mock<IMapper> _mapperMock;
        private readonly BookService _bookService;

        public BookServiceTests()
        {
            _bookRepositoryMock = new Mock<IBookRepository>();
            _bookImageRepositoryMock = new Mock<IBookImageRepository>();
            _bookFileRepositoryMock = new Mock<IBookFileRepository>();
            _bookAuthorRepositoryMock = new Mock<IBookAuthorRepository>();
            _mapperMock = new Mock<IMapper>();

            _bookService = new BookService(
                _bookRepositoryMock.Object,
                _bookImageRepositoryMock.Object,
                _bookFileRepositoryMock.Object,
                _mapperMock.Object,
                _bookAuthorRepositoryMock.Object);
        }

        [Fact]
        public async Task AddAsync_Should_Add_New_Book()
        {
            // Arrange
            var bookId = Guid.NewGuid();
            var ownerGuid = Guid.NewGuid();
            var authorsId = new List<Guid> { Guid.NewGuid() };
            var request = new BookAddRequest
            {
                Title = "New Book",
                Description = "Description",
                Genre = new List<string> { "Fiction" },
                PublicationDate = DateTime.Now
            };

            _bookRepositoryMock.Setup(repo => repo.AddAsync(It.IsAny<Book>()))
                .ReturnsAsync(true);

            _bookAuthorRepositoryMock.Setup(repo => repo.AddBookAuthor(It.IsAny<Guid>(), It.IsAny<IEnumerable<Guid>>()))
                .ReturnsAsync(true);

            // Act
            var result = await _bookService.AddAsync(request, authorsId, ownerGuid);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(request.Title, result.Title);

            _bookRepositoryMock.Verify(repo => repo.AddAsync(It.Is<Book>(b =>
                b.Title == request.Title &&
                b.Description == request.Description &&
                b.Genre == string.Join(", ", request.Genre) &&
                b.PublicationDate == request.PublicationDate &&
                b.OwnerID == ownerGuid)), Times.Once);

            _bookAuthorRepositoryMock.Verify(repo => repo.AddBookAuthor(It.IsAny<Guid>(), authorsId), Times.Once);
        }

        [Fact]
        public async Task GetAllAsync_Should_Return_All_Books()
        {
            // Arrange
            var books = new List<Book>
            {
                new Book { BookID = Guid.NewGuid(), Title = "Book 1" },
                new Book { BookID = Guid.NewGuid(), Title = "Book 2" }
            };

            _bookRepositoryMock.Setup(repo => repo.GetAllAsync())
                .ReturnsAsync(books);

            _mapperMock.Setup(mapper => mapper.Map<IEnumerable<BookResponse>>(books))
                .Returns(books.Select(b => new BookResponse { BookID = b.BookID, Title = b.Title }));

            // Act
            var result = await _bookService.GetAllAsync();

            // Assert
            Assert.NotNull(result);
            Assert.Equal(books.Count, result.Count());

            _bookRepositoryMock.Verify(repo => repo.GetAllAsync(), Times.Once);
        }

        [Fact]
        public void GetSortedBooks_Should_Return_Sorted_Books()
        {
            // Arrange
            var books = new List<BookResponse>
                {
                    new BookResponse { BookID = Guid.NewGuid(), Title = "Book B" },
                    new BookResponse { BookID = Guid.NewGuid(), Title = "Book A" }
                };

            var sortedBooks = books.OrderBy(b => b.Title);

            _mapperMock.Setup(mapper => mapper.Map<IEnumerable<BookResponse>>(sortedBooks))
                .Returns(sortedBooks);

            // Act
            var result = _bookService.GetSortedBooks(books, nameof(BookResponse.Title), SortOrderOptions.ASC);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(2, result.Count());
            Assert.Equal("Book A", result.First().Title);

            _mapperMock.Verify(mapper => mapper.Map<IEnumerable<BookResponse>>(It.IsAny<IEnumerable<BookResponse>>()), Times.Once);
        }


        [Fact]
        public async Task GetFilteredBooksAsync_Should_Return_Filtered_Books()
        {
            // Arrange
            var filter = new BookFilter
            {
                TitleOrAuthor = "Book",
                Genre = new List<string> { "Fiction" },
                PublicationDateFrom = DateTime.Now.AddYears(-1),
                MinRating = 4
            };

            var books = new List<Book>
            {
                new Book
                {
                    BookID = Guid.NewGuid(),
                    Title = "Book with Filter",
                    Genre = "Fiction",
                    PublicationDate = DateTime.Now.AddMonths(-6),
                    Rating = new List<Rating> { new Rating { Value = 5 } }
                }
            };

            _bookRepositoryMock.Setup(repo => repo.GetAllAsync())
                .ReturnsAsync(books);

            _mapperMock.Setup(mapper => mapper.Map<IEnumerable<BookResponse>>(books))
                .Returns(books.Select(b => new BookResponse
                {
                    BookID = b.BookID,
                    Title = b.Title,
                    Genre = b.Genre,
                    PublicationDate = b.PublicationDate.Value,
                    Rating = _mapperMock.Object.Map<RatingResponse>(b.Rating)
                }));

            // Act
            var result = await _bookService.GetFilteredBooksAsync(filter);

            // Assert
            Assert.NotNull(result);
            Assert.Single(result);

            _bookRepositoryMock.Verify(repo => repo.GetAllAsync(), Times.Once);
        }


        [Fact]
        public async Task GetByIdAsync_Should_Return_Book_By_Id()
        {
            // Arrange
            var bookId = Guid.NewGuid();
            var book = new Book
            {
                BookID = bookId,
                Title = "Book Title"
            };

            _bookRepositoryMock.Setup(repo => repo.GetByIdAsync(bookId))
                .ReturnsAsync(book);

            _mapperMock.Setup(mapper => mapper.Map<BookResponse>(book))
                .Returns(new BookResponse { BookID = book.BookID, Title = book.Title });

            // Act
            var result = await _bookService.GetByIdAsync(bookId);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(book.Title, result.Title);

            _bookRepositoryMock.Verify(repo => repo.GetByIdAsync(bookId), Times.Once);
        }


        [Fact]
        public async Task UpdateAsync_Should_Update_Existing_Book()
        {
            // Arrange
            var bookId = Guid.NewGuid();
            var request = new BookUpdateRequest
            {
                BookID = bookId,
                Title = "Updated Title",
                PublicationDate = DateTime.Now,
                Description = "Updated Description",
                Genre = "Updated Genre"
            };

            var existingBook = new Book
            {
                BookID = bookId,
                Title = "Old Title",
                PublicationDate = DateTime.Now.AddYears(-1),
                Description = "Old Description",
                Genre = "Old Genre"
            };

            _bookRepositoryMock.Setup(repo => repo.GetByIdAsync(bookId))
                .ReturnsAsync(existingBook);

            _bookRepositoryMock.Setup(repo => repo.UpdateAsync(It.IsAny<Book>()))
                .ReturnsAsync(true);

            // Act
            var result = await _bookService.UpdateAsync(request);

            // Assert
            Assert.True(result);

            _bookRepositoryMock.Verify(repo => repo.UpdateAsync(It.Is<Book>(b =>
                b.Title == request.Title &&
                b.PublicationDate == request.PublicationDate.Value &&
                b.Description == request.Description &&
                b.Genre == request.Genre)), Times.Once);

            _bookRepositoryMock.Verify(repo => repo.GetByIdAsync(bookId), Times.Once);
        }
    }
}