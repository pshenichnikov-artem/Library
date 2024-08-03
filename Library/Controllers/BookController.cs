using Library.Core.Domain.Entities;
using Library.Core.DTO;
using Library.Core.Enums;
using Library.Core.ServiceContracts;
using Library.Core.Services;
using Microsoft.AspNetCore.Components.Server.Circuits;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Security.Claims;

namespace Library.UI.Controllers
{
    [Route("books")]
    public class BookController : Controller
    {
        private readonly IBookServices _bookServices;
        private readonly IBookFileServices _bookFileServices;

        public BookController(IBookServices bookServices, IBookFileServices bookFileServices)
        {
            _bookServices = bookServices;
            _bookFileServices = bookFileServices;
        }

        [HttpGet]
        public async Task<IActionResult> Catalog([FromQuery] string? searchBy, [FromQuery] string? searchString, [FromQuery] string? sortBy = "Title", [FromQuery] SortOrderOptions orderBy = SortOrderOptions.DESC)
        {
            var books = await _bookServices.GetFilteredBook(searchBy, searchString);
            books = await _bookServices.GetSortedBook(books, sortBy, orderBy);

            ViewData["SearchBy"] = searchBy;
            ViewData["SearchString"] = searchString;
            ViewData["SortBy"] = sortBy;
            ViewData["SortOrder"] = orderBy.ToString();

            return View("BooksCatalog", books);
        }

        [Route("{id}")]
        [HttpGet]
        public async Task<IActionResult> DetailsBook([FromServices] IBookFileServices bookFileService, [FromRoute] Guid? id)
        {
            BookResponse? book = await _bookServices.GetBookByBookID(id);
            if (book == null)
                return NotFound();

            var fileList = await bookFileService.GetFileByBookID(id);
            if (fileList != null)
            {
                book.BookFileDocx = fileList.FirstOrDefault(f => f.FileType == "docx")?.BookFileID;
                book.BookFilePdf = fileList.FirstOrDefault(f => f.FileType == "pdf")?.BookFileID;
            }

            Image? image = await _bookFileServices.GetImageByID(book.CoverImageID);

            if(image != null)
                ViewBag.Base64Image = Convert.ToBase64String(System.IO.File.ReadAllBytes(Directory.GetCurrentDirectory() + "/Content/" + image.ImagePath));

            ViewBag.OwnerEmail = User.FindFirstValue(ClaimTypes.Email);
            return View("Book", book);
        }

        [Route("add")]
        [HttpGet]
        public IActionResult AddBook()
        {
            return View("Add");
        }

        [Route("add")]
        [HttpPost]
        public async Task<IActionResult> AddBook(BookAddRequest bookAddRequest)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Errors = ModelState.Values.SelectMany(temp => temp.Errors).Select(temp => temp.ErrorMessage);
                return View("Add", bookAddRequest);
            }

            Guid? imageID = null;
            if (bookAddRequest.ImageFile != null)
            {
                imageID = (await _bookFileServices.AddImageFile(bookAddRequest.ImageFile)).ImageID;
            }

            string ownerEmail = User.FindFirstValue(ClaimTypes.Email);
            BookResponse bookResponse = await _bookServices.AddBook(bookAddRequest, ownerEmail, imageID);

            if (bookAddRequest.DocxFile != null)
                await _bookFileServices.AddBookFile(bookResponse.BookID, bookAddRequest.DocxFile);
            if (bookAddRequest.PdfFile != null)
                await _bookFileServices.AddBookFile(bookResponse.BookID, bookAddRequest.PdfFile);

            return Redirect($"{bookResponse.BookID}");
        }


        [Route("{bookID}/update")]
        [HttpGet]
        public async Task<IActionResult> Update(Guid? bookID)
        {
            if (bookID == null)
                return BadRequest("Book ID is required.");

            var book = await _bookServices.GetBookByBookID(bookID.Value);
            if (book == null)
                return NotFound("Book not found.");

            var userEmail = User.FindFirstValue(ClaimTypes.Email);

            if (userEmail != book.OwnerBookEmail)
            {
                return Forbid("You are not authorized to update this book.");
            }

            var bookFile = await _bookFileServices.GetFileByBookID(book.BookID);

            ViewBag.BookImagePath = _bookFileServices.GetImageByID(book.CoverImageID);
            if (bookFile != null)
            {
                ViewBag.PdfFilePath = bookFile.FirstOrDefault(bf => bf.FileType == "pdf");
                ViewBag.DocxFilePath = bookFile.FirstOrDefault(bf => bf.FileType == "docx");
            }

            return View(book);
        }

        [Route("{bookid}/update")]
        [HttpPost]
        public async Task<IActionResult> Update(BookAddRequest bookAddRequest, [FromQuery] Guid bookID)
        {
            var book = await _bookServices.GetBookByBookID(bookID);
            if (book == null)
                return NotFound("Book not found.");

            var userEmail = User.FindFirstValue(ClaimTypes.Email);
            if (userEmail != book.OwnerBookEmail)
            {
                return Forbid("You are not authorized to update this book.");
            }

            return Redirect($"/{book.BookID}");
        }

        [Route("{bookid}/delete")]
        [HttpGet]
        public async Task<IActionResult> Delete(Guid? bookID)
        {
            if (bookID == null)
                return BadRequest("Book ID is required.");

            var book = await _bookServices.GetBookByBookID(bookID.Value);
            if (book == null)
                return NotFound("Book not found.");

            var userEmail = User.FindFirstValue(ClaimTypes.Email);

            if (userEmail != book.OwnerBookEmail)
            {
                return Forbid("You are not authorized to update this book.");
            }

            await _bookFileServices.DeleteBookFileByBookID(book.BookID);
            BookResponse? bookResponse = await _bookServices.DeleteBookByID(book.BookID);   

            return Redirect("/"); ;
        }


        [Route("download/{id}")]
        [HttpGet]
        public async Task<IActionResult> DownloadBook([FromServices] IBookFileServices fileServices, [FromRoute] Guid? id)
        {
            string? filePath = (await fileServices.GetFileById(id))?.FilePath;//TODO
            if (string.IsNullOrEmpty(filePath))
                return BadRequest();

            var fileFullPath = "C:/studies/Asp.Net_Core/Library/Library/Content/" + filePath;
            if (!System.IO.File.Exists(fileFullPath))
            {
                return NotFound();
            }

            var fileType = "application/octet-stream";
            string fileName = filePath.Substring(filePath.IndexOf("_") + 1);

            return PhysicalFile(fileFullPath, fileType, fileName);
        }
    }
}
