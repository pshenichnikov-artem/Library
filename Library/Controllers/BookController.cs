using AutoMapper.Configuration.Annotations;
using Library.Core.Domain.IdentityEntities;
using Library.Core.DTO.Book;
using Library.Core.Enums;
using Library.Core.ServiceContracts;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace Library.UI.Controllers
{
    [Route("/books")]
    public class BookController : Controller
    {
        private readonly IBookService _bookService;
        private readonly IBookImageService _bookImageService;
        private readonly IBookFileService _bookFileService;
        private readonly IAuthorService _authorService;
        private readonly UserManager<ApplicationUser> _userManager;

        public BookController(IBookService bookService, IBookImageService bookImageService, IBookFileService bookFileService, IAuthorService authorService, UserManager<ApplicationUser> userManager)
        {
            _bookService = bookService;
            _bookImageService = bookImageService;
            _bookFileService = bookFileService;
            _authorService = authorService;
            _userManager = userManager;
        }

        [HttpGet]
        public async Task<IActionResult> Catalog(BookFilter? filter, string? sortBy, SortOrderOptions? orderBy = SortOrderOptions.ASC)
        {
            if (filter != null)
            {
                ViewData["TitleOrAuthor"] = filter.TitleOrAuthor;
                ViewData["SearchString"] = filter.TitleOrAuthor;
                ViewData["Genre"] = filter.Genre;
                ViewData["PublicationDateTo"] = filter.PublicationDateTo?.ToString("yyyy-MM-dd");
                ViewData["PublicationDateFrom"] = filter.PublicationDateFrom?.ToString("yyyy-MM-dd");
                ViewData["MinRating"] = filter.MinRating != null ? filter.MinRating?.ToString().Replace(",", ".") : "0";
            }

            ViewData["SortBy"] = sortBy;
            ViewData["SortOrder"] = orderBy.ToString();

            if (ModelState.IsValid == false)
            {
                ViewBag.Errors = ModelState.Values.SelectMany(temp => temp.Errors).Select(temp => temp.ErrorMessage);
                return View();
            }

            IEnumerable<BookResponse> books;
            if (filter != null)
                books = await _bookService.GetFilteredBooksAsync(filter);
            else
                books = await _bookService.GetAllAsync();

            if (orderBy == null)
                orderBy = SortOrderOptions.ASC;

            books = _bookService.GetSortedBooks(books, sortBy, orderBy.Value);

            return View(books);
        }

        [Route("{bookID}")]
        [HttpGet]
        public async Task<IActionResult> BookDetails([FromRoute] Guid? bookID)
        {
            if (bookID == null)
                return NotFound();

            BookResponse? book = await _bookService.GetByIdAsync(bookID);
            if (book == null)
                return NotFound();

            ViewBag.IsOwner = User.FindFirstValue(ClaimTypes.Email) == book.OwnerEmail;
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (Guid.TryParse(userId, out Guid userGuid))
            {
                ViewBag.CurrentUserGuid = userGuid;
            }
            else
            {
                ViewBag.CurrentUserGuid = Guid.Empty;
            }

            return View(book);
        }

        [Route("add")]
        [HttpGet]
        public async Task<IActionResult> AddBook()
        {
            ViewBag.Authors = (await _authorService.GetAllAuthorsAsync());
            return View();
        }

        [Route("add")]
        [HttpPost]
        public async Task<IActionResult> AddBook(BookAddRequest bookAddRequest, IEnumerable<Guid>? authors)
        {
            var authorsList = (await _authorService.GetAllAuthorsAsync());

            if (authors == null || authors.Count() == 0)
                ModelState.AddModelError("All", "You didn't specify the authors");
            else if (authors.Count() > 10)
                ModelState.AddModelError("All", "You have specified too many authors (maximum 10)");
            else
            {
                var authorsHashSet = authorsList.Select(x => x.AuthorID).ToHashSet();
                if (!authors.All(x => authorsHashSet.Contains(x)))
                    ModelState.AddModelError("All", "You specified a non-existent author");
                else
                {
                    DateTime publicationDate = bookAddRequest.PublicationDate.Value;
                    DateTime minimumBirthDate = publicationDate.AddYears(-5);
                    DateTime maximumBirthDate = publicationDate.AddYears(-90);

                    if (authorsList.Where(x => authors.Contains(x.AuthorID)).Any(x => x.DateOfBirth > minimumBirthDate || x.DateOfBirth < maximumBirthDate))
                    {
                        ModelState.AddModelError("All", "Incorrect publication date of the book, the author must be at least 5 years old and less than 90 years old at the time of publication.");
                    }
                }
            }
            if (ModelState.IsValid == false)
            {
                ViewBag.Errors = ModelState.Values.SelectMany(temp => temp.Errors).Select(temp => temp.ErrorMessage);
                ViewBag.SelectedAuthors = authorsList.Where(x => authors.Contains(x.AuthorID));
                ViewBag.Authors = authorsList;
                return View(bookAddRequest);
            }

            string? ownerEmail = User.FindFirstValue(ClaimTypes.Email);
            if (string.IsNullOrEmpty(ownerEmail))
                return StatusCode(StatusCodes.Status500InternalServerError);

            var ownerGuid = (await _userManager.Users.FirstOrDefaultAsync(x => x.Email == ownerEmail))?.Id;

            if (ownerGuid == null)
                return StatusCode(503);

            var book = await _bookService.AddAsync(bookAddRequest, authors, ownerGuid);

            if (bookAddRequest.FileDocx != null)
                await _bookFileService.AddAsync(bookAddRequest.FileDocx, book.BookID);

            if (bookAddRequest.FilePdf != null)
                await _bookFileService.AddAsync(bookAddRequest.FilePdf, book.BookID);

            if (bookAddRequest.Image != null)
                await _bookImageService.AddImageAsync(bookAddRequest.Image, book.BookID);

            return Redirect($"{book.BookID}");
        }

        [Route("download/{id}")]
        [HttpGet]
        public async Task<IActionResult> DownloadBook([FromRoute] Guid? id)
        {
            if (id == null)
                return BadRequest();

            var file = await _bookFileService.GetFilesByIDAsync(id);
            if (file == null)
                return NotFound();

            var fileFullPath = "C:/studies/Asp.Net_Core/Library/Library/Content/files/" + file.FileName;
            if (!System.IO.File.Exists(fileFullPath))
            {
                return NotFound();
            }

            var fileType = "application/octet-stream";
            string fileName = file.FileName.Substring(file.FileName.IndexOf("_") + 1);

            return PhysicalFile(fileFullPath, fileType, fileName);
        }

        [Route("{bookid}/delete")]
        [HttpGet]
        public async Task<IActionResult> Delete(Guid? bookID)
        {
            if (bookID == null)
                return BadRequest("Book ID is required.");

            var book = await _bookService.GetByIdAsync(bookID.Value);
            if (book == null)
                return NotFound("Book not found.");

            var userEmail = User.FindFirstValue(ClaimTypes.Email);

            if (userEmail != book.OwnerEmail)
            {
                return Forbid("You are not authorized to update this book.");
            }

            await _bookFileService.DeleteFilesByBookIdAsync(book.BookID);
            await _bookImageService.DeleteImagesByBookIdAsync(book.BookID);
            BookResponse? bookResponse = await _bookService.DeleteAsync(book.BookID);

            return Redirect("/"); ;
        }
    }
}
