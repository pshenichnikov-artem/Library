using Library.Core.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.Core.Domain.RepositrotyContracts
{
    public interface IBookAuthorRepository
    {
        Task<bool> AddBookAuthor(Guid bookID, IEnumerable<Guid> authorsID);
        Task<bool> DeleteBookAuthor(BookAuthor bookAuthor);
    }
}
