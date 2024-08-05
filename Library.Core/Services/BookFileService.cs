using Library.Core.Domain.Entities;
using Library.Core.Domain.RepositrotyContracts;
using Library.Core.DTO.Book.BookFile;
using Library.Core.ServiceContracts;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;

namespace Library.Core.Services
{
    public class BookFileService : IBookFileService
    {
        private readonly IBookFileRepository _bookFileRepository;
        private readonly string _filesPath;

        public BookFileService(IBookFileRepository bookFileRepository, IWebHostEnvironment webHostEnvironment)
        {
            _bookFileRepository = bookFileRepository;
            _filesPath = Path.Combine(webHostEnvironment.ContentRootPath, "Content/files");
        }

        public async Task<BookFileResponse?> AddAsync(IFormFile file, Guid? bookId)
        {
            if (bookId == null)
                throw new ArgumentNullException(nameof(bookId));

            if (file == null || file.Length == 0)
                throw new ArgumentException("File cannot be null or empty", nameof(file));

            if (string.IsNullOrEmpty(file.FileName) || !file.FileName.EndsWith(".pdf", StringComparison.OrdinalIgnoreCase) && !file.FileName.EndsWith(".docx", StringComparison.OrdinalIgnoreCase))
                throw new ArgumentException("File must be a .pdf or .docx file", nameof(file));

            var fileName = $"{Guid.NewGuid()}_{file.FileName}";
            var filePath = Path.Combine(_filesPath, fileName);

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            var bookFile = new BookFile
            {
                BookFileID = Guid.NewGuid(),
                FileName = fileName,
                BookID = bookId.Value,
                FileType = Path.GetExtension(fileName).TrimStart('.').ToLower()
            };

            try
            {
                var success = await _bookFileRepository.AddAsync(bookFile);
                if (!success)
                    return null;

                return new BookFileResponse
                {
                    BookFileID = bookFile.BookFileID,
                    BookID = bookFile.BookID,
                    FileName = bookFile.FileName,
                    FileType = bookFile.FileType
                };
            }
            catch (Exception)
            {
                return null;
            }
        }

        public async Task<bool> UpdateAsync(Guid? fileId, IFormFile file)
        {
            if (fileId == null)
                throw new ArgumentNullException(nameof(fileId));

            if (file == null || file.Length == 0)
                throw new ArgumentException("File cannot be null or empty", nameof(file));

            if (string.IsNullOrEmpty(file.FileName) || !file.FileName.EndsWith(".pdf", StringComparison.OrdinalIgnoreCase) && !file.FileName.EndsWith(".docx", StringComparison.OrdinalIgnoreCase))
                throw new ArgumentException("File must be a .pdf or .docx file", nameof(file));

            var existingFile = await _bookFileRepository.GetByIdAsync(fileId.Value);
            if (existingFile == null)
                return false;

            var oldFilePath = Path.Combine(_filesPath, existingFile.FileName);
            if (File.Exists(oldFilePath))
                File.Delete(oldFilePath);

            var newFileName = $"{Guid.NewGuid()}_{file.FileName}";
            var newFilePath = Path.Combine(_filesPath, newFileName);

            using (var stream = new FileStream(newFilePath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            existingFile.FileName = newFileName;
            existingFile.FileType = Path.GetExtension(newFileName).TrimStart('.').ToLower();

            try
            {
                return await _bookFileRepository.UpdateAsync(existingFile);
            }
            catch (Exception)
            {
                return false;
            }
        }

        public async Task<bool> DeleteAsync(Guid? fileId)
        {
            if (fileId == null)
                throw new ArgumentNullException(nameof(fileId));

            var file = await _bookFileRepository.GetByIdAsync(fileId.Value);
            if (file == null)
                return false;

            var filePath = Path.Combine(_filesPath, file.FileName);
            if (File.Exists(filePath))
                File.Delete(filePath);

            try
            {
                return await _bookFileRepository.DeleteAsync(fileId.Value);
            }
            catch (Exception)
            {
                return false;
            }
        }

        public async Task<IEnumerable<BookFileResponse>> GetFilesByBookIdAsync(Guid? bookId)
        {
            if (bookId == null)
                throw new ArgumentNullException(nameof(bookId));

            var files = await _bookFileRepository.GetAllAsync();
            var bookFiles = files.Where(file => file.BookID == bookId);

            return bookFiles.Select(file => new BookFileResponse
            {
                BookFileID = file.BookFileID,
                BookID = file.BookID,
                FileName = file.FileName,
                FileType = file.FileType
            });
        }

        public async Task<bool> DeleteFilesByBookIdAsync(Guid? bookId)
        {
            if (bookId == null)
                throw new ArgumentNullException(nameof(bookId));

            var files = await _bookFileRepository.GetAllAsync();
            var bookFiles = files.Where(file => file.BookID == bookId).ToList();

            foreach (var file in bookFiles)
            {
                var filePath = Path.Combine(_filesPath, file.FileName);
                if (File.Exists(filePath))
                    File.Delete(filePath);

                try
                {
                    await _bookFileRepository.DeleteAsync(file.BookFileID);
                }
                catch (Exception)
                {
                    return false;
                }
            }

            return true;
        }

        public async Task<byte[]> GetFileBytesAsync(string fileName)
        {
            if (string.IsNullOrEmpty(fileName))
                throw new ArgumentNullException(nameof(fileName), "File name cannot be null or empty");

            var filePath = Path.Combine(_filesPath, fileName);

            if (!File.Exists(filePath))
                throw new FileNotFoundException("File not found", fileName);

            return await File.ReadAllBytesAsync(filePath);
        }

        public async Task<BookFileResponse?> GetFilesByIDAsync(Guid? bookFileID)
        {
            if (bookFileID == null)
                throw new ArgumentNullException(nameof(bookFileID));

            var bookFile = await _bookFileRepository.GetByIdAsync(bookFileID.Value);
            if (bookFile == null)
                return null;

            return new BookFileResponse
                {
                    BookFileID = bookFile.BookFileID,
                    BookID = bookFile.BookID,
                    FileName = bookFile.FileName,
                    FileType = bookFile.FileType
                };
        }
    }
}
