using AutoMapper;
using Library.Core.Domain.Entities;
using Library.Core.Domain.RepositrotyContracts;
using Library.Core.DTO;
using Library.Core.Enums;
using Library.Core.ServiceContracts;
using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace Library.Core.Services
{
    public class BookServices : IBookServices
    {
        private readonly IBookRepository _bookRepository;
        private readonly IMapper _mapper;

        public BookServices(IBookRepository bookRepository, IMapper mapper)
        {
            _bookRepository = bookRepository;
            _mapper = mapper;
        }

        public virtual async Task<BookResponse> AddBook(BookAddRequest bookAddRequest, Guid? imageID = null)
        {
            if(bookAddRequest == null)
            {
                throw new ArgumentNullException(nameof(bookAddRequest));
            }

            ValidationContext validationContext = new ValidationContext(bookAddRequest);
            List<ValidationResult> validationResults = new List<ValidationResult>();

            bool isValid = Validator.TryValidateObject(bookAddRequest, validationContext, validationResults, true);
            if (!isValid)
            {
                throw new ArgumentException(validationResults.FirstOrDefault()?.ErrorMessage);
            }

            Book book = _mapper.Map<Book>(bookAddRequest);
            book.BookID = Guid.NewGuid();
            if(imageID != null)
                book.CoverImageID = imageID.Value;
            await _bookRepository.AddBookAsync(book);

            return _mapper.Map<BookResponse>(book);
        }

        public async Task<bool> DeleteBookByID(Guid? bookID)
        {
            if (bookID == null)
            {
                throw new ArgumentNullException(nameof(bookID));
            }

            Book? person = await _bookRepository.GetBookByIDAsync(bookID.Value);
            if (person == null)
                return false;

            return await _bookRepository.DeleteBookByIDAsync(bookID.Value);
        }

        public virtual async Task<List<BookResponse>> GetAllBooks()
        {
            var books = await _bookRepository.GetAllBookAsync();

            return books.Select(b => _mapper.Map<BookResponse>(b)).ToList();
        }

        public virtual async Task<BookResponse?> GetBookByBookID(Guid? bookID)
        {
            if(bookID == null)
                return null;

            var book = await _bookRepository.GetBookByIDAsync(bookID.Value);
            if(book == null)
                return null;

            return _mapper.Map<BookResponse>(book);
        }

        public virtual async Task<List<BookResponse>> GetFilteredBook(string? searchBy, string? searchString)
        {
            if (searchString == null)
                return await GetAllBooks();

            Func<Book, string>? getProperty = searchBy switch
            {
                nameof(Book.Title) => b => b.Title,
                nameof(Book.Description) => b => b.Description,
                nameof(Book.PublicationDate) => b => b.PublicationDate.Value.ToString("yyyy-MM-dd"),
                nameof(Book.Genre) => b => b.Genre,
                nameof(Book.Author) => b => b.Author,
                _ => null
            };

            if (getProperty == null)
                return await GetAllBooks();

            List<Book> books = await _bookRepository.GetFilteredBookAsync(b=> getProperty(b).Contains(searchString));

            return books.Select(b => _mapper.Map<BookResponse>(b)).ToList();
        }

        public async Task<List<BookResponse>> GetSortedBook(List<BookResponse> books, string sortBy, SortOrderOptions sortOrder)
        {
            if (string.IsNullOrEmpty(sortBy))
                return books;

            Func<BookResponse, string>? getProperty = sortBy switch
            {
                nameof(BookResponse.Title) => b => b.Title,
                nameof(BookResponse.Description) => b => b.Description,
                nameof(BookResponse.PublicationDate) => b => b.PublicationDate,
                nameof(BookResponse.Genre) => b => b.Genre,
                nameof(BookResponse.Author) => b => b.Author,
                _ => null
            };

            if (getProperty == null)
                return books;

            if(sortOrder == SortOrderOptions.ASC)
            {
               return books.OrderBy(b => getProperty(b)).ToList();
            }
            else
            {
                return books.OrderByDescending(b => getProperty(b)).ToList();
            }
        }

        public async Task<BookResponse> UpdateBook(Book? bookRequest)//todo сделать BookUpdateRequest
            //todo файл, //todo картинка
        {
            if (bookRequest == null)
                throw new ArgumentNullException(nameof(bookRequest));

            ValidationContext validationContext = new ValidationContext(bookRequest);
            List<ValidationResult> validationResults = new List<ValidationResult>();

            bool isValid = Validator.TryValidateObject(bookRequest, validationContext, validationResults, true);
            if (!isValid)
            {
                throw new ArgumentException(validationResults.FirstOrDefault()?.ErrorMessage);
            }

            //get matching person object to update
            Book? matchingPerson = await _bookRepository.GetBookByIDAsync(bookRequest.BookID);
            if (matchingPerson == null)
            {
                throw new InvalidDataException("Given person id doesn't exist");
            }

            //TODO изменить поля

            await _bookRepository.UpdateBookAsync(matchingPerson);
            return _mapper.Map<BookResponse>(matchingPerson);
        }
    }
}
