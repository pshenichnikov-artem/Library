using Library.Core.Domain.Entities;
using Library.Core.Domain.RepositrotyContracts;
using Library.Core.DTO.Book.BookImage;
using Library.Core.ServiceContracts;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;

namespace Library.Core.Services
{
    public class BookImageService : IBookImageService
    {
        private readonly IBookImageRepository _bookImageRepository;
        private readonly string _imagesPath;

        public BookImageService(IBookImageRepository bookImageRepository, IWebHostEnvironment webHostEnvironment)
        {
            _bookImageRepository = bookImageRepository;
            _imagesPath = webHostEnvironment.WebRootPath + "/bookImages";
        }

        public async Task<BookImageResponse?> AddImageAsync(IFormFile file, Guid? bookId)
        {
            if(bookId == null)
                throw new ArgumentNullException(nameof(bookId));

            if (file == null || file.Length == 0)
                throw new ArgumentException("File cannot be null or empty", nameof(file));

            if (string.IsNullOrEmpty(file.FileName) || !file.FileName.EndsWith(".jpg", StringComparison.OrdinalIgnoreCase) && !file.FileName.EndsWith(".png", StringComparison.OrdinalIgnoreCase))
                throw new ArgumentException("File must be a .jpg or .png file", nameof(file));

            var fileName = $"{Guid.NewGuid()}_{file.FileName}";
            var filePath = Path.Combine(_imagesPath, fileName);

            Directory.CreateDirectory(_imagesPath);

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            var bookImage = new BookImage
            {
                BookImageID = Guid.NewGuid(),
                FileName = fileName,
                BookID = bookId.Value
            };

            try
            {
                var success = await _bookImageRepository.AddAsync(bookImage);
                if (!success)
                    return null;

                return new BookImageResponse
                {
                    BookImageID = bookImage.BookImageID,
                    FileName = bookImage.FileName,
                    FilePath = $"/bookImages/{fileName}"
                };
            }
            catch (Exception)
            {
                return null;
            }
        }

        public async Task<bool> UpdateImageAsync(Guid? imageId, IFormFile file)
        {
            if (imageId == null)
                throw new ArgumentNullException(nameof(imageId));

            if (file == null || file.Length == 0)
                throw new ArgumentException("File cannot be null or empty", nameof(file));

            if (string.IsNullOrEmpty(file.FileName) || !file.FileName.EndsWith(".jpg", StringComparison.OrdinalIgnoreCase) && !file.FileName.EndsWith(".png", StringComparison.OrdinalIgnoreCase))
                throw new ArgumentException("File must be a .jpg or .png file", nameof(file));

            var existingImage = await _bookImageRepository.GetByIdAsync(imageId.Value);
            if (existingImage == null)
                return false;

            var oldFilePath = Path.Combine(_imagesPath, existingImage.FileName);
            if (File.Exists(oldFilePath))
                File.Delete(oldFilePath);

            var newFileName = $"{Guid.NewGuid()}_{file.FileName}";
            var newFilePath = Path.Combine(_imagesPath, newFileName);

            using (var stream = new FileStream(newFilePath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            existingImage.FileName = newFileName;

            try
            {
                return await _bookImageRepository.UpdateAsync(existingImage);
            }
            catch (Exception)
            {
                return false;
            }
        }

        public async Task<bool> DeleteImageAsync(Guid? imageId)
        {
            if(imageId == null)
                throw new ArgumentNullException(nameof(imageId));

            var image = await _bookImageRepository.GetByIdAsync(imageId.Value);
            if (image == null)
                return false;

            var filePath = Path.Combine(_imagesPath, image.FileName);
            if (File.Exists(filePath))
                File.Delete(filePath);

            try
            {
                return await _bookImageRepository.DeleteAsync(imageId.Value);
            }
            catch (Exception)
            {
                return false;
            }
        }

        public async Task<IEnumerable<BookImageResponse>> GetImagesByBookIdAsync(Guid? bookId)
        {
            if(bookId == null)
                throw new ArgumentNullException(nameof(bookId));

            var images = await _bookImageRepository.GetAllAsync();
            var bookImages = images.Where(img => img.BookID == bookId);

            return bookImages.Select(img => new BookImageResponse
            {
                BookImageID = img.BookImageID,
                FileName = img.FileName,
                FilePath = $"/bookImages/{img.FileName}"
            });
        }

        public async Task<bool> DeleteImagesByBookIdAsync(Guid? bookId)
        {
            if(bookId == null)
                throw new ArgumentNullException(nameof(bookId));

            var images = await _bookImageRepository.GetAllAsync();
            var bookImages = images.Where(img => img.BookID == bookId).ToList();

            foreach (var image in bookImages)
            {
                var filePath = Path.Combine(_imagesPath, image.FileName);
                if (File.Exists(filePath))
                    File.Delete(filePath);

                try
                {
                    await _bookImageRepository.DeleteAsync(image.BookImageID);
                }
                catch (Exception)
                {
                    return false;
                }
            }

            return true;
        }
    }
}
