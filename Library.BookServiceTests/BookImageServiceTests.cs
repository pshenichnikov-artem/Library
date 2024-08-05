using Library.Core.Domain.Entities;
using Library.Core.Domain.RepositrotyContracts;
using Library.Core.Services;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.ServicesTests
{
    public class BookImageServiceTests
    {
        private readonly Mock<IBookImageRepository> _mockBookImageRepository;
        private readonly BookImageService _service;
        private readonly string _imagesPath;

        public BookImageServiceTests()
        {
            _mockBookImageRepository = new Mock<IBookImageRepository>();
            var mockWebHostEnvironment = new Mock<IWebHostEnvironment>();
            _imagesPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "bookImages");
            mockWebHostEnvironment.Setup(env => env.WebRootPath).Returns(Directory.GetCurrentDirectory() + "/wwwroot");

            _service = new BookImageService(_mockBookImageRepository.Object, mockWebHostEnvironment.Object);

            if (!Directory.Exists(_imagesPath))
                Directory.CreateDirectory(_imagesPath);
        }

        [Fact]
        public async Task AddImageAsync_ShouldThrowArgumentException_WhenFileIsInvalid()
        {
            var file = new Mock<IFormFile>();
            file.Setup(f => f.FileName).Returns("test.txt");
            file.Setup(f => f.Length).Returns(1);

            var bookId = Guid.NewGuid();

            await Assert.ThrowsAsync<ArgumentException>(() => _service.AddImageAsync(file.Object, bookId));
        }

        [Fact]
        public async Task DeleteImageAsync_ShouldReturnTrue_WhenImageIsDeletedSuccessfully()
        {
            var imageId = Guid.NewGuid();
            var existingImage = new BookImage
            {
                BookImageID = imageId,
                FileName = "test.jpg",
                BookID = Guid.NewGuid()
            };

            _mockBookImageRepository.Setup(repo => repo.GetByIdAsync(imageId)).ReturnsAsync(existingImage);
            _mockBookImageRepository.Setup(repo => repo.DeleteAsync(imageId)).ReturnsAsync(true);

            var result = await _service.DeleteImageAsync(imageId);

            Assert.True(result);
        }

        [Fact]
        public async Task DeleteImagesByBookIdAsync_ShouldReturnTrue_WhenAllImagesAreDeletedSuccessfully()
        {
            var bookId = Guid.NewGuid();
            var images = new List<BookImage>
            {
                new BookImage { BookImageID = Guid.NewGuid(), FileName = "image1.jpg", BookID = bookId },
                new BookImage { BookImageID = Guid.NewGuid(), FileName = "image2.jpg", BookID = bookId }
            };

            _mockBookImageRepository.Setup(repo => repo.GetAllAsync()).ReturnsAsync(images);
            _mockBookImageRepository.Setup(repo => repo.DeleteAsync(It.IsAny<Guid>())).ReturnsAsync(true);

            var result = await _service.DeleteImagesByBookIdAsync(bookId);

            Assert.True(result);
        }

        [Fact]
        public async Task GetImagesByBookIdAsync_ShouldReturnImages_WhenImagesExistForBook()
        {
            var bookId = Guid.NewGuid();
            var images = new List<BookImage>
            {
                new BookImage { BookImageID = Guid.NewGuid(), FileName = "image1.jpg", BookID = bookId },
                new BookImage { BookImageID = Guid.NewGuid(), FileName = "image2.jpg", BookID = bookId }
            };

            _mockBookImageRepository.Setup(repo => repo.GetAllAsync()).ReturnsAsync(images);

            var result = await _service.GetImagesByBookIdAsync(bookId);

            Assert.NotNull(result);
            Assert.Equal(2, result.Count());
        }
    }
}
