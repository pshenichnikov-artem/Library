using AutoMapper;
using Library.Core.Domain.Entities;
using Library.Core.Domain.RepositrotyContracts;
using Library.Core.DTO.Author.AuthorImage;
using Library.Core.Services;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Moq;

namespace Library.ServicesTests
{
    public class AuthorImageServiceTests
    {
        private readonly string _imagesPath;
        private readonly Mock<IAuthorImageRepository> _authorImageRepositoryMock;
        private readonly Mock<IWebHostEnvironment> _webHostEnvironmentMock;
        private readonly AuthorImageService _authorImageService;

        public AuthorImageServiceTests()
        {
            _authorImageRepositoryMock = new Mock<IAuthorImageRepository>();
            _webHostEnvironmentMock = new Mock<IWebHostEnvironment>();
            _webHostEnvironmentMock.Setup(m => m.WebRootPath).Returns(Path.Combine(Directory.GetCurrentDirectory(), "wwwroot"));
            _imagesPath = Path.Combine(_webHostEnvironmentMock.Object.WebRootPath, "authorImages");
            _authorImageService = new AuthorImageService(_authorImageRepositoryMock.Object, _webHostEnvironmentMock.Object);
        }

        [Fact]
        public async Task AddImageAsync_Should_Add_New_Image()
        {
            // Arrange
            var authorId = Guid.NewGuid();
            var fileMock = new Mock<IFormFile>();
            var content = "Hello World from a Fake File";
            var fileName = "test.jpg";
            var ms = new MemoryStream();
            var writer = new StreamWriter(ms);
            writer.Write(content);
            writer.Flush();
            ms.Position = 0;
            fileMock.Setup(_ => _.OpenReadStream()).Returns(ms);
            fileMock.Setup(_ => _.FileName).Returns(fileName);
            fileMock.Setup(_ => _.Length).Returns(ms.Length);

            if (!Directory.Exists(_imagesPath))
            {
                Directory.CreateDirectory(_imagesPath);
            }

            _authorImageRepositoryMock.Setup(repo => repo.AddAsync(It.IsAny<AuthorImage>())).ReturnsAsync(true);

            // Act
            var result = await _authorImageService.AddImageAsync(fileMock.Object, authorId);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(fileName, result.ImageName.Substring(result.ImageName.IndexOf("_") + 1));

            // Cleanup
            var filePath = _imagesPath + "/" + result.ImageName;
            if (File.Exists(filePath))
            {
                File.Delete(filePath);
            }

            if (Directory.Exists(_imagesPath))
            {
                Directory.Delete(_imagesPath);
            }
        }

        [Fact]
        public async Task DeleteImageAsync_Should_Delete_Existing_Image()
        {
            // Arrange
            var imageId = Guid.NewGuid();
            var existingImage = new AuthorImage
            {
                AuthorImageID = imageId,
                FileName = "existing.jpg",
                AuthorID = Guid.NewGuid()
            };

            _authorImageRepositoryMock.Setup(x => x.GetByAuthorIdAsync(imageId))
                .ReturnsAsync(existingImage);
            _authorImageRepositoryMock.Setup(x => x.DeleteAsync(It.IsAny<AuthorImage>()))
                .ReturnsAsync(true);

            // Act
            var result = await _authorImageService.DeleteImageAsync(imageId);

            // Assert
            Assert.True(result);
        }

        [Fact]
        public async Task GetImagesByAuthorIdAsync_Should_Return_Images()
        {
            // Arrange
            var authorId = Guid.NewGuid();
            var images = new List<AuthorImage>
            {
                new AuthorImage { AuthorImageID = Guid.NewGuid(), FileName = "image1.jpg", AuthorID = authorId },
                new AuthorImage { AuthorImageID = Guid.NewGuid(), FileName = "image2.jpg", AuthorID = authorId }
            };

            _authorImageRepositoryMock.Setup(x => x.GetAllAsync())
                .ReturnsAsync(images);

            // Act
            var result = await _authorImageService.GetImagesByAuthorIdAsync(authorId);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(2, result.Count());
            Assert.Contains(result, img => img.ImageName == "image1.jpg");
            Assert.Contains(result, img => img.ImageName == "image2.jpg");
        }

        [Fact]
        public async Task DeleteImagesByUserIdAsync_Should_Delete_All_Images()
        {
            // Arrange
            var authorId = Guid.NewGuid();
            var images = new List<AuthorImage>
            {
                new AuthorImage { AuthorImageID = Guid.NewGuid(), FileName = "image1.jpg", AuthorID = authorId },
                new AuthorImage { AuthorImageID = Guid.NewGuid(), FileName = "image2.jpg", AuthorID = authorId }
            };

            _authorImageRepositoryMock.Setup(x => x.GetAllAsync())
                .ReturnsAsync(images);
            _authorImageRepositoryMock.Setup(x => x.DeleteAsync(It.IsAny<AuthorImage>()))
                .ReturnsAsync(true);

            // Act
            var result = await _authorImageService.DeleteImagesByUserIdAsync(authorId);

            // Assert
            Assert.True(result);
        }
    }
}
