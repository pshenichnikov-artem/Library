using AutoMapper;
using Library.Core.Domain.Entities;
using Library.Core.Domain.RepositrotyContracts;
using Library.Core.DTO.Book;
using Library.Core.Enums;
using Library.Core.ServiceContracts;
using System.Linq;

namespace Library.Core.Services
{
    public class BookService : IBookService
    {
        private readonly IBookRepository _bookRepository;
        private readonly IBookImageRepository _bookImageRepository;
        private readonly IBookFileRepository _bookFileRepository;
        private readonly IBookAuthorRepository _bookAuthorRepository;
        private readonly IMapper _mapper;

        public BookService(IBookRepository bookRepository, IBookImageRepository bookImageRepository,
                       IBookFileRepository bookFileRepository, IMapper mapper, IBookAuthorRepository bookAuthorRepository)
        {
            _bookRepository = bookRepository ?? throw new ArgumentNullException(nameof(bookRepository));
            _bookImageRepository = bookImageRepository ?? throw new ArgumentNullException(nameof(bookImageRepository));
            _bookFileRepository = bookFileRepository ?? throw new ArgumentNullException(nameof(bookFileRepository));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _bookAuthorRepository = bookAuthorRepository ?? throw new ArgumentNullException(nameof(bookAuthorRepository));
        }

        public async Task<Book?> AddAsync(BookAddRequest? request, IEnumerable<Guid> authorsID, Guid? ownerGuid)
        {
            if (ownerGuid == null)
                throw new ArgumentNullException(nameof(ownerGuid));

            if (authorsID == null || authorsID.Count() == 0)
                throw new ArgumentNullException(nameof(authorsID));

            if (request == null)
                throw new ArgumentNullException(nameof(request), "Request cannot be null");

            var book = new Book
            {
                BookID = Guid.NewGuid(),
                Title = request.Title,
                Description = request.Description,
                Genre = request.Genre != null && request.Genre.Count != 0 ? string.Join(", ", request.Genre) : null,
                PublicationDate = request.PublicationDate,
                OwnerID = ownerGuid.Value,
            };

            var successAddBook = await _bookRepository.AddAsync(book);
            var successAddBookAuthor = await _bookAuthorRepository.AddBookAuthor(book.BookID, authorsID);
            if (!successAddBook || !successAddBook)
                throw new ArgumentException("Error add in db");

            return book;
        }

        public async Task<BookResponse?> DeleteAsync(Guid? bookId)
        {
            if (bookId == null)
                throw new ArgumentNullException(nameof(bookId), "Book ID cannot be null");

            var book = await _bookRepository.GetByIdAsync(bookId.Value);

            if (book == null)
                return null;

            await _bookRepository.DeleteAsync(book);
            return _mapper.Map<BookResponse>(book);
        }

        public async Task<IEnumerable<BookResponse>> GetAllAsync()
        {
            var books = await _bookRepository.GetAllAsync();
            return _mapper.Map<IEnumerable<BookResponse>>(books);
        }

        public async Task<BookResponse?> GetByIdAsync(Guid? bookId)
        {
            if (bookId == null)
                throw new ArgumentNullException(nameof(bookId), "Book ID cannot be null");

            var book = await _bookRepository.GetByIdAsync(bookId.Value);
            if (book == null)
                return null;

            return _mapper.Map<BookResponse>(book);
        }

        public async Task<IEnumerable<BookResponse>> GetFilteredBooksAsync(BookFilter? filter)
        {
            if (filter == null)
                throw new ArgumentNullException(nameof(filter), "Filter cannot be null");

            var books = await _bookRepository.GetAllAsync();

            if (!string.IsNullOrEmpty(filter.TitleOrAuthor))
            {
                books = books.Where(b => b.Title.Contains(filter.TitleOrAuthor, StringComparison.OrdinalIgnoreCase)
                || b.BookAuthors.Any(ba => ba.Author.FirstName.Contains(filter.TitleOrAuthor, StringComparison.OrdinalIgnoreCase)
                         || ba.Author.LastName.Contains(filter.TitleOrAuthor, StringComparison.OrdinalIgnoreCase)));
            }

            if (filter.Genre != null && filter.Genre.Count != 0)
            {
                books = books.Where(b => b.Genre != null
                ? b.Genre.Contains(string.Join(" ", filter.Genre), StringComparison.OrdinalIgnoreCase) : false);
            }

            if (filter.PublicationDateFrom.HasValue)
            {
                books = books.Where(b => b.PublicationDate >= filter.PublicationDateFrom.Value);
            }

            if (filter.PublicationDateTo.HasValue)
            {
                books = books.Where(b => b.PublicationDate <= filter.PublicationDateTo.Value);
            }

            if (filter.MinRating.HasValue)
            {
                books = books.Where(b => b.Rating != null ? b.Rating.Average(r => r.Value) >= filter.MinRating.Value : false);
            }

            var filteredBooks = books.AsEnumerable();
            return _mapper.Map<IEnumerable<BookResponse>>(filteredBooks);
        }

        public IEnumerable<BookResponse> GetSortedBooks(IEnumerable<BookResponse>? books, string? sortField = "Title", SortOrderOptions sortOrder = SortOrderOptions.ASC)
        {
            if (books == null)
                throw new ArgumentNullException(nameof(books));

            if (!Enum.IsDefined(typeof(SortOrderOptions), sortOrder))
                throw new ArgumentException("Invalid sort order", nameof(sortOrder));

            Func<BookResponse, object> getProperty = sortField switch
            {
                nameof(BookResponse.Title) => b => b.Title,
                nameof(BookResponse.Genre) => b => b.Genre,
                nameof(BookResponse.PublicationDate) => b => b.PublicationDate.ToString("yyyy-MM-dd"),
                nameof(BookResponse.Rating) => b => b.Rating.Value.ToString(),
                _ => b => b.Title
            };

            var sortedBooks = sortOrder == SortOrderOptions.ASC
                    ? books.OrderBy(getProperty)
                    : books.OrderByDescending(getProperty);


            return _mapper.Map<IEnumerable<BookResponse>>(sortedBooks.AsEnumerable());
        }

        public async Task<bool> UpdateAsync(BookUpdateRequest? request)
        {
            if (request == null)
                throw new ArgumentNullException(nameof(request), "Request cannot be null");

            if (string.IsNullOrEmpty(request.Title))
                throw new ArgumentException("Title cannot be null or empty", nameof(request.Title));

            if (!request.PublicationDate.HasValue)
                throw new ArgumentException("Publication date must be provided", nameof(request.PublicationDate));

            var book = await _bookRepository.GetByIdAsync(request.BookID);
            if (book == null)
                return false;

            book.Title = request.Title;
            book.PublicationDate = request.PublicationDate.Value;
            book.Description = request.Description;
            book.Genre = request.Genre;

            try
            {
                return await _bookRepository.UpdateAsync(book);
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}
