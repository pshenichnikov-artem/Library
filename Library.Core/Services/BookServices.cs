using AutoMapper;
using Library.Core.Domain.Entities;
using Library.Core.Domain.RepositrotyContracts;
using Library.Core.DTO;
using Library.Core.Enums;
using Library.Core.ServiceContracts;
using Microsoft.AspNetCore.Hosting;
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

        public virtual async Task<BookResponse> AddBook(BookAddRequest bookAddRequest, string ownerEmail, Guid? imageID = null)
        {
            if(bookAddRequest == null)
            {
                throw new ArgumentNullException(nameof(bookAddRequest));
            }

            if(string.IsNullOrWhiteSpace(ownerEmail))
                throw new ArgumentNullException(nameof(ownerEmail));

            ValidationContext validationContext = new ValidationContext(bookAddRequest);
            List<ValidationResult> validationResults = new List<ValidationResult>();

            bool isValid = Validator.TryValidateObject(bookAddRequest, validationContext, validationResults, true);
            if (!isValid)
            {
                throw new ArgumentException(validationResults.FirstOrDefault()?.ErrorMessage);
            }

            Book book = _mapper.Map<Book>(bookAddRequest);
            book.BookID = Guid.NewGuid();
            book.OwnerBookEmail = ownerEmail;
            if(imageID != null)
                book.CoverImageID = imageID.Value;
            await _bookRepository.AddBookAsync(book);

            return _mapper.Map<BookResponse>(book);
        }

        public async Task<BookResponse?> DeleteBookByID(Guid? bookID)
        {
            if (bookID == null)
            {
                throw new ArgumentNullException(nameof(bookID));
            }

            Book? book = await _bookRepository.GetBookByIDAsync(bookID.Value);
            if (book == null)
                return null;

            await _bookRepository.DeleteBookByIDAsync(bookID.Value);
            return _mapper.Map<BookResponse>(book);
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
                _ =>  b => $"{b.Title} {b.Description} {(b.PublicationDate.HasValue ? b.PublicationDate.Value.ToString("yyyy-MM-dd") : string.Empty)} {b.Genre} {b.Author}"
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
               return books.OrderByDescending(getProperty).ToList();
            }
            else
            {
                return books.OrderBy(getProperty).ToList();
            }
        }

        //public async Task<BookResponse> UpdateBook(Guid bookID, BookAddRequest bookAddRequest, string ownerEmail)
        //{
        //    if (bookAddRequest == null)
        //        throw new ArgumentNullException(nameof(bookAddRequest));

        //    // Проверка валидности запроса
        //    ValidationContext validationContext = new ValidationContext(bookAddRequest);
        //    List<ValidationResult> validationResults = new List<ValidationResult>();
        //    bool isValid = Validator.TryValidateObject(bookAddRequest, validationContext, validationResults, true);
        //    if (!isValid)
        //        throw new ArgumentException(validationResults.FirstOrDefault()?.ErrorMessage);

        //    // Получение существующей книги
        //    Book existingBook = await _bookRepository.GetBookByIDAsync(bookID);
        //    if (existingBook == null)
        //        throw new InvalidOperationException("Book not found");

        //    if (existingBook.OwnerBookEmail != ownerEmail)
        //        throw new UnauthorizedAccessException("You are not authorized to update this book.");

        //    // Обработка нового изображения
        //    if (bookAddRequest.ImageFile != null)
        //    { 
        //        var newImage = await _b.AddImageFile(bookAddRequest.ImageFile);
        //        existingBook.CoverImageID = newImage.ImageID;
        //    }

        //    // Обработка нового PDF файла
        //    if (bookAddRequest.PdfFile != null)
        //    {
        //        var pdfFile = await _bookFileServices.AddBookFile(existingBook.BookID, bookAddRequest.PdfFile);
        //        existingBook.PdfFileID = pdfFile.BookFileID;
        //    }

        //    // Обработка нового DOCX файла
        //    if (bookAddRequest.DocxFile != null)
        //    {
        //        var docxFile = await _bookFileServices.AddBookFile(existingBook.BookID, bookAddRequest.DocxFile);
        //        existingBook.DocxFileID = docxFile.BookFileID;
        //    }

        //    // Обновление информации о книге
        //    existingBook.Title = bookAddRequest.Title;
        //    existingBook.Description = bookAddRequest.Description;
        //    existingBook.Genre = bookAddRequest.Genre;
        //    existingBook.PublicationDate = DateTime.Parse(bookAddRequest.PublicationDate);
        //    existingBook.Author = bookAddRequest.Author;

        //    // Удаление старых файлов, если они существуют и не были заменены
        //    if (bookAddRequest.DocxFile == null && existingBook.DocxFileID != null)
        //    {
        //        await _bookFileServices.DeleteBookFileByBookID(existingBook.BookID);
        //        existingBook.DocxFileID = null;
        //    }

        //    if (bookAddRequest.PdfFile == null && existingBook.PdfFileID != null)
        //    {
        //        await _bookFileServices.DeleteBookFileByBookID(existingBook.BookID);
        //        existingBook.PdfFileID = null;
        //    }

        //    await _bookRepository.UpdateBookAsync(existingBook);

        //    return _mapper.Map<BookResponse>(existingBook);
        //}

    }
}
