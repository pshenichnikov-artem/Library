using Library.Core.Domain.Entities;
using Library.Core.DTO;
using Library.Core.ServiceContracts;
using Library.Core.Services;
using Microsoft.AspNetCore.Mvc;

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
        public async Task<IActionResult> Catalog()
        {
            var books = await _bookServices.GetAllBooks();
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

            ViewBag.Image = book.CoverImageID;

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
        public async Task<IActionResult> AddBook(BookAddRequest bookAddRequest, IFormFile docxFile, IFormFile pdfFile, IFormFile bookImage)
        {
            if (ModelState.IsValid == false)
            {
                ViewBag.Errors = ModelState.Values.SelectMany(temp => temp.Errors).Select(temp => temp.ErrorMessage);
                return View("Add", bookAddRequest);
            }
            //todo картинка
            bool docxIsValid = docxFile != null && docxFile.Length > 0 && Path.GetExtension(docxFile.FileName).ToLowerInvariant() == ".docx";
            bool pdfIsValid = pdfFile != null && pdfFile.Length > 0 && Path.GetExtension(pdfFile.FileName).ToLowerInvariant() == ".pdf";
            if (!docxIsValid && !pdfIsValid)
            {
                ModelState.AddModelError("File", "Please upload at least one file: DOCX or PDF.");
                ViewBag.Errors = ModelState.Values.SelectMany(temp => temp.Errors).Select(temp => temp.ErrorMessage);
                return View("Add", bookAddRequest);
            }

            BookResponse bookResponse = await _bookServices.AddBook(bookAddRequest);
            if(docxIsValid)
                await _bookFileServices.AddBookFile(bookResponse.BookID, docxFile);
            if (pdfIsValid)
                await _bookFileServices.AddBookFile(bookResponse.BookID, pdfFile);

            return Redirect($"books/{bookResponse.BookID}");
        }

        //update


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
