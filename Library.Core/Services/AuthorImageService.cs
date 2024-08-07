using Library.Core.Domain.Entities;
using Library.Core.Domain.RepositrotyContracts;
using Library.Core.DTO.Author.AuthorImage;
using Library.Core.ServiceContracts;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;

namespace Library.Core.Services
{
    public class AuthorImageService : IAuthorImageService
    {
        private readonly IAuthorImageRepository _authorImageRepository;
        private readonly string _imagesPath;

        public AuthorImageService(IAuthorImageRepository authorImageRepository, IWebHostEnvironment webHostEnvironment)
        {
            _authorImageRepository = authorImageRepository;
            _imagesPath = webHostEnvironment.WebRootPath + "/authorImages";
        }

        public async Task<AuthorImageResponse?> AddImageAsync(IFormFile file, Guid? authorId)
        {
            if (authorId == null)
                throw new ArgumentNullException(nameof(authorId));

            if (file == null || file.Length == 0)
                throw new ArgumentException("File cannot be null or empty", nameof(file));

            if (string.IsNullOrEmpty(file.FileName) || !file.FileName.EndsWith(".jpg", StringComparison.OrdinalIgnoreCase) && !file.FileName.EndsWith(".png", StringComparison.OrdinalIgnoreCase))
                throw new ArgumentException("File must be a .jpg or .png file", nameof(file));

            var fileName = $"{Guid.NewGuid()}_{file.FileName}";
            var filePath = Path.Combine(_imagesPath, fileName);

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            var userImage = new AuthorImage
            {
                AuthorImageID = Guid.NewGuid(),
                FileName = fileName,
                AuthorID = authorId.Value
            };

            try
            {
                var success = await _authorImageRepository.AddAsync(userImage);
                if (!success)
                    return null;

                return new AuthorImageResponse
                {
                    ImageName = userImage.FileName
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

            var existingImage = await _authorImageRepository.GetByAuthorIdAsync(imageId.Value);
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
                return await _authorImageRepository.UpdateAsync(existingImage);
            }
            catch (Exception)
            {
                return false;
            }
        }

        public async Task<bool> DeleteImageAsync(Guid? imageId)
        {
            if (imageId == null)
                throw new ArgumentNullException(nameof(imageId));

            var image = await _authorImageRepository.GetByAuthorIdAsync(imageId.Value);
            if (image == null)
                return false;

            var filePath = Path.Combine(_imagesPath, image.FileName);
            if (File.Exists(filePath))
                File.Delete(filePath);

            try
            {
                return await _authorImageRepository.DeleteAsync(image);
            }
            catch (Exception)
            {
                return false;
            }
        }

        public async Task<IEnumerable<AuthorImageResponse>> GetImagesByAuthorIdAsync(Guid? authorId)
        {
            if (authorId == null)
                throw new ArgumentNullException(nameof(authorId));

            var images = await _authorImageRepository.GetAllAsync();
            var userImages = images.Where(img => img.AuthorID == authorId);

            return userImages.Select(img => new AuthorImageResponse
            {
                ImageName = img.FileName,
            });
        }

        public async Task<bool> DeleteImagesByUserIdAsync(Guid? authorId)
        {
            if (authorId == null)
                throw new ArgumentNullException(nameof(authorId));

            var images = await _authorImageRepository.GetAllAsync();
            var userImages = images.Where(img => img.AuthorID == authorId).ToList();

            foreach (var image in userImages)
            {
                var filePath = Path.Combine(_imagesPath, image.FileName);
                if (File.Exists(filePath))
                    File.Delete(filePath);

                try
                {
                    await _authorImageRepository.DeleteAsync(image);
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
