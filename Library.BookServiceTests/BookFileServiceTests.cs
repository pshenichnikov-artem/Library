
using Library.Core.Domain.Entities;
using Library.Core.Domain.RepositrotyContracts;
using Library.Core.Services;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Moq;

namespace Library.ServicesTests
{
    public class BookFileServiceTests
    {
        private readonly Mock<IBookFileRepository> _bookFileRepositoryMock;
        private readonly BookFileService _bookFileService;
        private readonly string _testFilesPath;

        public BookFileServiceTests()
        {
            _bookFileRepositoryMock = new Mock<IBookFileRepository>();
            var webHostEnvironmentMock = new Mock<IWebHostEnvironment>();
            _testFilesPath = Path.Combine(Directory.GetCurrentDirectory(), "Content/files");
            Directory.CreateDirectory(_testFilesPath);
            webHostEnvironmentMock.Setup(env => env.ContentRootPath).Returns(Directory.GetCurrentDirectory());
            _bookFileService = new BookFileService(_bookFileRepositoryMock.Object, webHostEnvironmentMock.Object);
        }

        [Fact]
        public async Task UpdateAsync_Should_Update_Existing_File()
        {
            // Arrange
            var fileId = Guid.NewGuid();
            var existingFile = new BookFile
            {
                BookFileID = fileId,
                FileName = "old_file.pdf",
                BookID = Guid.NewGuid(),
                FileType = "pdf"
            };

            var newFileName = "new_file.pdf";
            var fileMock = new Mock<IFormFile>();
            var content = "Fake content";
            var ms = new MemoryStream();
            var writer = new StreamWriter(ms);
            writer.Write(content);
            writer.Flush();
            ms.Position = 0;
            fileMock.Setup(_ => _.OpenReadStream()).Returns(ms);
            fileMock.Setup(_ => _.FileName).Returns(newFileName);
            fileMock.Setup(_ => _.Length).Returns(ms.Length);

            var updatedFile = new BookFile
            {
                BookFileID = fileId,
                FileName = $"{Guid.NewGuid()}_{newFileName}", // Simulate new file name
                BookID = existingFile.BookID,
                FileType = Path.GetExtension(newFileName).TrimStart('.').ToLower()
            };

            _bookFileRepositoryMock.Setup(repo => repo.GetByIdAsync(fileId)).ReturnsAsync(existingFile);
            _bookFileRepositoryMock.Setup(repo => repo.UpdateAsync(It.IsAny<BookFile>())).ReturnsAsync(true);
            _bookFileRepositoryMock.Setup(repo => repo.GetByIdAsync(fileId)).ReturnsAsync(updatedFile); // Return updated file with new name

            // Act
            var result = await _bookFileService.UpdateAsync(fileId, fileMock.Object);

            // Assert
            Assert.True(result);

            // Verify file deletion and addition
            var oldFilePath = Path.Combine(_testFilesPath, existingFile.FileName);
            var newFilePath = Path.Combine(_testFilesPath, updatedFile.FileName);

            // Ensure old file was deleted
            Assert.False(File.Exists(oldFilePath), "Old file was not deleted");

            // Ensure new file was created
            Assert.True(File.Exists(newFilePath), "New file was not created");

            // Cleanup
            if (File.Exists(newFilePath))
            {
                File.Delete(newFilePath);
            }
        }

        [Fact]
        public async Task UpdateAsync_Should_Return_False_When_File_Not_Exists()
        {
            // Arrange
            var fileId = Guid.NewGuid();
            var fileMock = new Mock<IFormFile>();
            fileMock.Setup(_ => _.FileName).Returns("file.pdf");
            fileMock.Setup(_ => _.Length).Returns(1);

            _bookFileRepositoryMock.Setup(repo => repo.GetByIdAsync(fileId)).ReturnsAsync((BookFile)null);

            // Act
            var result = await _bookFileService.UpdateAsync(fileId, fileMock.Object);

            // Assert
            Assert.False(result);
        }

        [Fact]
        public async Task UpdateAsync_Should_Throw_Exception_When_File_Is_Null()
        {
            // Arrange
            var fileId = Guid.NewGuid();

            // Act & Assert
            await Assert.ThrowsAsync<ArgumentException>(() => _bookFileService.UpdateAsync(fileId, null));
        }

        [Fact]
        public async Task UpdateAsync_Should_Throw_Exception_When_File_Is_Not_Supported()
        {
            // Arrange
            var fileId = Guid.NewGuid();
            var fileMock = new Mock<IFormFile>();
            fileMock.Setup(_ => _.FileName).Returns("file.txt");
            fileMock.Setup(_ => _.Length).Returns(1);

            _bookFileRepositoryMock.Setup(repo => repo.GetByIdAsync(fileId)).ReturnsAsync(new BookFile());

            // Act & Assert
            await Assert.ThrowsAsync<ArgumentException>(() => _bookFileService.UpdateAsync(fileId, fileMock.Object));
        }
    }
}
