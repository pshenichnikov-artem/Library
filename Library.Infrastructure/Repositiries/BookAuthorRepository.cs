
using Library.Core.Domain.Entities;
using Library.Core.Domain.RepositrotyContracts;
using Library.Infrastructure.DbContext;

namespace Library.Infrastructure.Repositiries
{
    public class BookAuthorRepository : IBookAuthorRepository
    {
        private readonly ApplicationDbContext _db;
        
        public BookAuthorRepository(ApplicationDbContext db)
        {
            _db = db;
        } 

        public async Task<bool> AddBookAuthor(Guid bookID, IEnumerable<Guid> authorsID)
        {
            foreach (var authorID in authorsID)
            {
                var bookAuthor = new BookAuthor() { BookAuthorID = Guid.NewGuid(), BookID = bookID, AuthorID = authorID };
                await _db.BookAuthors.AddAsync(bookAuthor);
            }
            return await _db.SaveChangesAsync() == authorsID.Count();
        }

        public async Task<bool> DeleteBookAuthor(BookAuthor bookAuthor)
        {
            _db.Remove(bookAuthor);
            return await _db.SaveChangesAsync() > 0;
        }
    }
}
