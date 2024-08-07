using Library.Core.Domain.Entities;
using Library.Core.Domain.RepositrotyContracts;
using Library.Core.DTO;
using Library.Core.ServiceContracts;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;

namespace Library.Core.Services
{
    public class UserImageService : IUserImageService
    {
        private readonly IUserImageRepository _userImageRepository;
        private readonly string _imagesPath;

        public UserImageService(IUserImageRepository userImageRepository, IWebHostEnvironment webHostEnvironment)
        {
            _userImageRepository = userImageRepository;
            _imagesPath = webHostEnvironment.WebRootPath + "/userImages";
        }

        public async Task<UserImageResponse?> AddImageAsync(IFormFile file, Guid? userId)
        {
            if (userId == null)
                throw new ArgumentNullException(nameof(userId));

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

            var userImage = new UserImage
            {
                UserImageID = Guid.NewGuid(),
                FileName = fileName,
                UserID = userId.Value
            };

            try
            {
                var success = await _userImageRepository.AddAsync(userImage);
                if (!success)
                    return null;

                return new UserImageResponse
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

            var existingImage = await _userImageRepository.GetByUserIdAsync(imageId.Value);
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
                return await _userImageRepository.UpdateAsync(existingImage);
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

            var image = await _userImageRepository.GetByUserIdAsync(imageId.Value);
            if (image == null)
                return false;

            var filePath = Path.Combine(_imagesPath, image.FileName);
            if (File.Exists(filePath))
                File.Delete(filePath);

            try
            {
                return await _userImageRepository.DeleteAsync(imageId.Value);
            }
            catch (Exception)
            {
                return false;
            }
        }

        public async Task<IEnumerable<UserImageResponse>> GetImagesByUserIdAsync(Guid? userId)
        {
            if (userId == null)
                throw new ArgumentNullException(nameof(userId));

            var images = await _userImageRepository.GetAllAsync();
            var userImages = images.Where(img => img.UserID == userId);

            return userImages.Select(img => new UserImageResponse
            {
                ImageName = img.FileName,
            });
        }

        public async Task<bool> DeleteImagesByUserIdAsync(Guid? userId)
        {
            if (userId == null)
                throw new ArgumentNullException(nameof(userId));

            var images = await _userImageRepository.GetAllAsync();
            var userImages = images.Where(img => img.UserID == userId).ToList();

            foreach (var image in userImages)
            {
                var filePath = Path.Combine(_imagesPath, image.FileName);
                if (File.Exists(filePath))
                    File.Delete(filePath);

                try
                {
                    await _userImageRepository.DeleteAsync(image.UserImageID);
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
