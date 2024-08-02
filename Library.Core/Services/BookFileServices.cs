using Library.Core.Domain.Entities;
using Library.Core.Domain.RepositrotyContracts;
using Library.Core.ServiceContracts;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;

namespace Library.Core.Services
{
    public class BookFileServices : IBookFileServices
    {
        private readonly IBookFileRepository _bookFileRepository;
        private readonly IWebHostEnvironment _webHostEnvironment;
        public BookFileServices(IBookFileRepository bookFileRepository, IWebHostEnvironment webHostEnvironment)
        {
            _bookFileRepository = bookFileRepository;
            _webHostEnvironment = webHostEnvironment;
        }

        public async Task<BookFile?> AddBookFile(Guid? bookID, IFormFile? file)
        {
            if(bookID == null)
                throw new ArgumentNullException(nameof(bookID));
            if(file == null)
                throw new ArgumentNullException(nameof(file));
            if (file == null || file.Length == 0)
                throw new ArgumentNullException("File can't be blank");

            string ext = Path.GetExtension(file.FileName).ToLowerInvariant();
            if (ext != ".docx" && ext != ".pdf")
                throw new InvalidDataException("Error extension file");
            
            BookFile bookFile = new BookFile()
            {
                BookID = bookID.Value,
                BookFileID = Guid.NewGuid(),
                FileType = ext.Substring(ext.IndexOf('.') + 1),
                FilePath = $"files/{Guid.NewGuid()}_{file.FileName}"
            };

            string fullPath = _webHostEnvironment.ContentRootPath + "Content/" + bookFile.FilePath;

            using (var stream = new FileStream(fullPath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            return bookFile;
        }

        public async Task<BookFile?> GetFileById(Guid? fileID)
        {
            if (fileID == null)
                return null;

            BookFile? file = await _bookFileRepository.GetFileByID(fileID.Value);
            if (file == null)
                return null;

            return file;
        }

        public async Task<List<BookFile>?> GetFileByBookID(Guid? bookID)
        {
            if (bookID == null)
                return null;

            List<BookFile>? fileList = await _bookFileRepository.GetFileByBookID(bookID.Value);
            if (fileList == null)
                return null;

            return fileList;
        }

        public async Task<Image?> GetImageByBookID(Guid? imageID)
        {
            if (imageID == null)
                return null;

            Image? image = await _bookFileRepository.GetImageByID(imageID.Value);
            if (image == null)
                return null;

            return image;
        }

        public Task<Image> AddImageFile(IFormFile file)
        {
            throw new NotImplementedException();
        }
    }
}
