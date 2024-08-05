//using AutoMapper;
//using Library.Core.Automapper;
//using Library.Core.Domain.Entities;
//using Library.Core.Domain.RepositrotyContracts;
//using Library.Core.DTO;
//using Library.Core.DTO.Book;
//using Library.Core.Enums;
//using Library.Core.Services;
//using Moq;

//namespace Library.BookServiceTests
//{
//    public class BookServiceTests
//    {
//        private readonly Mock<IBookRepository> _mockBookRepository;
//        private readonly Mock<IBookImageRepository> _mockBookImageRepository;
//        private readonly Mock<IBookFileRepository> _mockBookFileRepository;
//        private readonly IMapper _mapper;
//        private readonly BookService _bookService;

//        public BookServiceTests()
//        {
//            _mockBookRepository = new Mock<IBookRepository>();
//            _mockBookImageRepository = new Mock<IBookImageRepository>();
//            _mockBookFileRepository = new Mock<IBookFileRepository>();

//            var config = new MapperConfiguration(cfg =>
//            {
//                cfg.AddProfile<AutomapperProfile>();
//            });

//            _mapper = config.CreateMapper();
//            _bookService = new BookService(
//                _mockBookRepository.Object,
//                _mockBookImageRepository.Object,
//                _mockBookFileRepository.Object,
//                _mapper);
//        }

//        [Fact]
//        public async Task GetByIdAsync_ShouldThrowArgumentNullException_WhenBookIdIsNull()
//        {
//            await Assert.ThrowsAsync<ArgumentNullException>(() => _bookService.GetByIdAsync(null));
//        }

//        [Fact]
//        public async Task GetByIdAsync_ShouldReturnNull_WhenBookNotFound()
//        {
//            _mockBookRepository
//                .Setup(repo => repo.GetByIdAsync(It.IsAny<Guid>()))
//                .ReturnsAsync((Book?)null);

//            var result = await _bookService.GetByIdAsync(Guid.NewGuid());

//            Assert.Null(result);
//        }

//        [Fact]
//        public async Task GetFilteredBooksAsync_ShouldThrowArgumentNullException_WhenFilterIsNull()
//        {
//            await Assert.ThrowsAsync<ArgumentNullException>(() => _bookService.GetFilteredBooksAsync(null));
//        }

//        [Fact]
//        public async Task GetFilteredBooksAsync_ShouldFilterBooksByTitle()
//        {
//            var books = new List<Book>
//            {
//                new Book { Title = "Book1", Genre = "Genre1", PublicationDate = DateTime.Today, Rating = new Rating(){Value = 5 } },
//                new Book { Title = "Book2", Genre = "Genre2", PublicationDate = DateTime.Today, Rating = new Rating(){Value = 5 } }
//            };

//            _mockBookRepository
//                .Setup(repo => repo.GetAllAsync())
//                .ReturnsAsync(books);

//            var filter = new BookFilter { TitleOrAuthor = "Book1" };

//            var result = await _bookService.GetFilteredBooksAsync(filter);

//            Assert.Single(result);
//            Assert.Equal("Book1", result.First().Title);
//        }

//        [Fact]
//        public void GetSortedBooksAsync_ShouldThrowArgumentNullException_WhenBooksAreNull()
//        {
//            Assert.Throws<ArgumentNullException>(() => _bookService.GetSortedBooks(null, "Title", SortOrderOptions.ASC));
//        }

//        [Fact]
//        public void GetSortedBooksAsync_ShouldSortByTitle_WhenSortFieldIsInvalid()
//        {
//            var books = new List<BookResponse>
//            {
//                new BookResponse { Title = "BookB", Genre = "Genre1", PublicationDate = DateTime.Today, Rating = new Core.DTO.RatingResponse() { Value = 5 } },
//                new BookResponse { Title = "BookA", Genre = "Genre2", PublicationDate = DateTime.Today, Rating = new RatingResponse() { Value = 3 } }
//            };

//            var sortedBooks = _bookService.GetSortedBooks(books, "InvalidProperty", SortOrderOptions.ASC);

//            var sortedBookTitles = sortedBooks.Select(b => b.Title).ToList();
//            Assert.Equal(new List<string> { "BookA", "BookB" }, sortedBookTitles);
//        }

//        [Fact]
//        public void GetSortedBooksAsync_ShouldSortBooksByTitle()
//        {
//            var books = new List<BookResponse>
//            {
//                new BookResponse { Title = "BookB", Genre = "Genre1", PublicationDate = DateTime.Today, Rating = new RatingResponse() { Value = 5 } },
//                new BookResponse { Title = "BookA", Genre = "Genre2", PublicationDate = DateTime.Today, Rating = new Core.DTO.RatingResponse() { Value = 3 } }
//            };

//            var result = _bookService.GetSortedBooks(books, "Title", SortOrderOptions.ASC).ToList();

//            Assert.Equal("BookA", result.First().Title);
//            Assert.Equal("BookB", result.Last().Title);
//        }

//        [Fact]
//        public async Task DeleteAsync_ShouldReturnBook_WhenBookExists()
//        {
//            // Arrange
//            var bookId = Guid.NewGuid();
//            var book = new Book { BookID = bookId };

//            _mockBookRepository.Setup(repo => repo.GetByIdAsync(bookId))
//                .ReturnsAsync(book);
//            _mockBookRepository.Setup(repo => repo.DeleteAsync(book))
//                .ReturnsAsync(book);

//            // Act
//            var result = await _bookService.DeleteAsync(bookId);

//            // Assert
//            Assert.NotNull(result);
//            Assert.Equal(bookId, result.BookID);
//        }

//        [Fact]
//        public async Task DeleteAsync_ShouldReturnNull_WhenBookDoesNotExist()
//        {
//            // Arrange
//            var bookId = Guid.NewGuid();

//            _mockBookRepository.Setup(repo => repo.GetByIdAsync(bookId))
//                .ReturnsAsync((Book)null);

//            // Act
//            var result = await _bookService.DeleteAsync(bookId);

//            // Assert
//            Assert.Null(result);
//        }

//        [Fact]
//        public async Task DeleteAsync_ShouldThrowArgumentNullException_WhenBookIdIsNull()
//        {
//            // Act & Assert
//            await Assert.ThrowsAsync<ArgumentNullException>(() => _bookService.DeleteAsync(null));
//        }

//        [Fact]
//        public async Task AddAsync_ShouldReturnBook_WhenRequestIsValid()
//        {
//            // Arrange
//            var request = new BookAddRequest
//            {
//                Title = "Book Title",
//                Description = "Book Description",
//                Genre = new List<string?> { "Genre" },
//                PublicationDate = DateTime.Today
//            };

//            var book = new Book
//            {
//                BookID = Guid.NewGuid(),
//                Title = request.Title,
//                Description = request.Description,
//                Genre = string.Join(", ", request.Genre),
//                PublicationDate = request.PublicationDate
//            };

//            _mockBookRepository.Setup(repo => repo.AddAsync(It.IsAny<Book>()))
//                .ReturnsAsync(true);

//            // Act
//            var result = await _bookService.AddAsync(request, Guid.NewGuid());

//            // Assert
//            Assert.NotNull(result);
//            Assert.Equal(request.Title, result.Title);
//            Assert.Equal(request.Description, result.Description);
//            Assert.Equal(string.Join(", ", request.Genre), result.Genre);
//            Assert.Equal(request.PublicationDate, result.PublicationDate);
//        }

//        [Fact]
//        public async Task AddAsync_ShouldThrowArgumentNullException_WhenRequestIsNull()
//        {
//            // Act & Assert
//            await Assert.ThrowsAsync<ArgumentNullException>(() => _bookService.AddAsync(null, null));
//        }

//        [Fact]
//        public async Task AddAsync_ShouldReturnNull_WhenRepositoryFails()
//        {
//            // Arrange
//            var request = new BookAddRequest
//            {
//                Title = "Book Title",
//                Description = "Book Description",
//                Genre = new List<string?> { "Genre" },
//                PublicationDate = DateTime.Today
//            };

//            _mockBookRepository.Setup(repo => repo.AddAsync(It.IsAny<Book>()))
//                .ThrowsAsync(new Exception("Database error"));

//            // Act
//            var result = await _bookService.AddAsync(request, Guid.NewGuid());

//            // Assert
//            Assert.Null(result);
//        }

//        [Fact]
//        public async Task DeleteAsync_ShouldReturnNull_WhenRepositoryFails()
//        {
//            // Arrange
//            var bookId = Guid.NewGuid();
//            var book = new Book { BookID = bookId };

//            _mockBookRepository.Setup(repo => repo.GetByIdAsync(bookId))
//                .ReturnsAsync(book);
//            _mockBookRepository.Setup(repo => repo.DeleteAsync(book))
//                .ThrowsAsync(new Exception("Database error"));

//            // Act
//            var result = await _bookService.DeleteAsync(bookId);

//            // Assert
//            Assert.Null(result);
//        }

//        [Fact]
//        public async Task UpdateAsync_ShouldThrowArgumentNullException_WhenRequestIsNull()
//        {
//            await Assert.ThrowsAsync<ArgumentNullException>(() => _bookService.UpdateAsync(null));
//        }

//        [Fact]
//        public async Task UpdateAsync_ShouldThrowArgumentException_WhenTitleIsNullOrEmpty()
//        {
//            var request = new BookUpdateRequest
//            {
//                BookID = Guid.NewGuid(),
//                Title = null,
//                PublicationDate = DateTime.Now
//            };

//            await Assert.ThrowsAsync<ArgumentException>(() => _bookService.UpdateAsync(request));
//        }

//        [Fact]
//        public async Task UpdateAsync_ShouldThrowArgumentException_WhenPublicationDateIsNotProvided()
//        {
//            var request = new BookUpdateRequest
//            {
//                BookID = Guid.NewGuid(),
//                Title = "Valid Title",
//                PublicationDate = null
//            };

//            await Assert.ThrowsAsync<ArgumentException>(() => _bookService.UpdateAsync(request));
//        }

//        [Fact]
//        public async Task UpdateAsync_ShouldReturnFalse_WhenBookNotFound()
//        {
//            var request = new BookUpdateRequest
//            {
//                BookID = Guid.NewGuid(),
//                Title = "Valid Title",
//                PublicationDate = DateTime.Now
//            };

//            _mockBookRepository.Setup(repo => repo.GetByIdAsync(request.BookID))
//                .ReturnsAsync((Book)null);

//            var result = await _bookService.UpdateAsync(request);

//            Assert.False(result);
//        }

//        [Fact]
//        public async Task UpdateAsync_ShouldReturnTrue_WhenUpdateIsSuccessful()
//        {
//            var bookId = Guid.NewGuid();
//            var request = new BookUpdateRequest
//            {
//                BookID = bookId,
//                Title = "Updated Title",
//                PublicationDate = DateTime.Now,
//                Description = "Updated Description",
//                Genre = "Updated Genre"
//            };

//            var book = new Book { BookID = bookId };

//            _mockBookRepository.Setup(repo => repo.GetByIdAsync(bookId))
//                .ReturnsAsync(book);

//            _mockBookRepository.Setup(repo => repo.UpdateAsync(book))
//                .ReturnsAsync(true);

//            var result = await _bookService.UpdateAsync(request);

//            Assert.True(result);
//            Assert.Equal(request.Title, book.Title);
//            Assert.Equal(request.PublicationDate, book.PublicationDate);
//            Assert.Equal(request.Description, book.Description);
//            Assert.Equal(request.Genre, book.Genre);
//        }

//        [Fact]
//        public async Task UpdateAsync_ShouldReturnFalse_WhenUpdateFails()
//        {
//            var bookId = Guid.NewGuid();
//            var request = new BookUpdateRequest
//            {
//                BookID = bookId,
//                Title = "Updated Title",
//                PublicationDate = DateTime.Now
//            };

//            var book = new Book { BookID = bookId };

//            _mockBookRepository.Setup(repo => repo.GetByIdAsync(bookId))
//                .ReturnsAsync(book);

//            _mockBookRepository.Setup(repo => repo.UpdateAsync(book))
//                .ThrowsAsync(new Exception("Update failed"));

//            var result = await _bookService.UpdateAsync(request);

//            Assert.False(result);
//        }
//    }
//}