using Library.Core.Domain.Entities;
using Library.Core.Domain.RepositrotyContracts;
using Library.Infrastructure.DbContext;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Library.Infrastructure.Repositiries
{
    public class BookRepository : IBookRepository
    {
        private readonly ApplicationDbContext _db;

        public BookRepository(ApplicationDbContext db)
        {
            _db = db;
        }

        public async Task<Book> AddBookAsync(Book book)
        {
            _db.Books.Add(book);
            await _db.SaveChangesAsync();
            return book;
        }

        public async Task<List<Book>> GetAllBookAsync()
        {
            return await _db.Books
                .ToListAsync();
        }

        public async Task<Book?> GetBookByIDAsync(Guid bookID)
        {
            return await _db.Books
                .SingleOrDefaultAsync(b => b.BookID == bookID);
        }

        public async Task<List<Book>> GetFilteredBookAsync(Expression<Func<Book, bool>> predicate)
        {
            return await _db.Books
                .Where(predicate)
                .ToListAsync();
        }

        public async Task<bool> DeleteBookByIDAsync(Guid bookID)
        {
            _db.Books.RemoveRange(_db.Books.Where(a => a.BookID == bookID));
            return (await _db.SaveChangesAsync()) > 0;
        }

        public async Task<Book> UpdateBookAsync(Book book)
        {
            Book? matchingBook = await _db.Books.FirstOrDefaultAsync(a => a.BookID == book.BookID);
            if (matchingBook == null)
                return book;

            matchingBook.Title = book.Title;
            matchingBook.Description = book.Description;
            matchingBook.Genre = book.Genre;
            matchingBook.PublicationDate = book.PublicationDate;
            matchingBook.Author = book.Author;

            return book;
        }
    }
}
