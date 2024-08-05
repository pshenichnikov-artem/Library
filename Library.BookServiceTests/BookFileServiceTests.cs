
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
        private readonly Mock<IBookFileRepository> _mockBookFileRepository;
        private readonly BookFileService _bookFileService;
        private readonly string _testFilesPath;

        public BookFileServiceTests()
        {
            _mockBookFileRepository = new Mock<IBookFileRepository>();
            var webHostEnvironment = new Mock<IWebHostEnvironment>();
            _testFilesPath = Path.Combine(Path.GetTempPath(), "Content/files");

            // Setup the mock for IWebHostEnvironment
            webHostEnvironment.Setup(env => env.WebRootPath).Returns(Path.GetTempPath());

            _bookFileService = new BookFileService(_mockBookFileRepository.Object, webHostEnvironment.Object);
        }

        [Fact]
        public async Task AddAsync_ShouldReturnNull_WhenFileCannotBeAdded()
        {
            // Arrange
            var fileName = "testfile.pdf";
            var formFile = new Mock<IFormFile>();
            formFile.Setup(f => f.FileName).Returns(fileName);
            formFile.Setup(f => f.Length).Returns(1);

            var bookId = Guid.NewGuid();

            _mockBookFileRepository
                .Setup(repo => repo.AddAsync(It.IsAny<BookFile>()))
                .ReturnsAsync(false);

            // Act
            var result = await _bookFileService.AddAsync(formFile.Object, bookId);

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public async Task DeleteFileAsync_ShouldReturnTrue_WhenFileIsDeletedSuccessfully()
        {
            // Arrange
            var fileId = Guid.NewGuid();
            var fileName = "filetodelete.pdf";

            var fileToDelete = new BookFile
            {
                BookFileID = fileId,
                FileName = fileName,
                BookID = Guid.NewGuid(),
                FileType = "pdf"
            };

            _mockBookFileRepository
                .Setup(repo => repo.GetByIdAsync(fileId))
                .ReturnsAsync(fileToDelete);

            _mockBookFileRepository
                .Setup(repo => repo.DeleteAsync(fileId))
                .ReturnsAsync(true);

            // Act
            var result = await _bookFileService.DeleteAsync(fileId);

            // Assert
            Assert.True(result);
        }

        [Fact]
        public async Task GetFilesByBookIdAsync_ShouldReturnFiles_WhenFilesExist()
        {
            // Arrange
            var bookId = Guid.NewGuid();
            var files = new List<BookFile>
        {
            new BookFile { BookFileID = Guid.NewGuid(), BookID = bookId, FileName = "file1.pdf", FileType = "pdf" },
            new BookFile { BookFileID = Guid.NewGuid(), BookID = bookId, FileName = "file2.docx", FileType = "docx" }
        };

            _mockBookFileRepository
                .Setup(repo => repo.GetAllAsync())
                .ReturnsAsync(files);

            // Act
            var result = await _bookFileService.GetFilesByBookIdAsync(bookId);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(2, result.Count());
            Assert.All(result, file => Assert.Equal(bookId, file.BookID));
        }

        [Fact]
        public async Task DeleteFilesByBookIdAsync_ShouldReturnTrue_WhenFilesAreDeletedSuccessfully()
        {
            // Arrange
            var bookId = Guid.NewGuid();
            var files = new List<BookFile>
        {
            new BookFile { BookFileID = Guid.NewGuid(), BookID = bookId, FileName = "file1.pdf", FileType = "pdf" },
            new BookFile { BookFileID = Guid.NewGuid(), BookID = bookId, FileName = "file2.docx", FileType = "docx" }
        };

            _mockBookFileRepository
                .Setup(repo => repo.GetAllAsync())
                .ReturnsAsync(files);

            _mockBookFileRepository
                .Setup(repo => repo.DeleteAsync(It.IsAny<Guid>()))
                .ReturnsAsync(true);

            // Act
            var result = await _bookFileService.DeleteFilesByBookIdAsync(bookId);

            // Assert
            Assert.True(result);
        }

        [Fact]
        public async Task GetFileBytesAsync_ShouldReturnFileBytes_WhenFileExists()
        {
            // Arrange
            var fileName = "testfile.pdf";
            var fileContent = new byte[] { 1, 2, 3, 4, 5 };
            var filePath = Path.Combine(_testFilesPath, fileName);
            Directory.CreateDirectory(_testFilesPath);
            await File.WriteAllBytesAsync(filePath, fileContent);

            // Act
            var result = await _bookFileService.GetFileBytesAsync(fileName);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(fileContent, result);
        }

        [Fact]
        public async Task GetFileBytesAsync_ShouldThrowFileNotFoundException_WhenFileDoesNotExist()
        {
            // Arrange
            var fileName = "nonexistentfile.pdf";

            // Act & Assert
            await Assert.ThrowsAsync<FileNotFoundException>(() => _bookFileService.GetFileBytesAsync(fileName));
        }
    }
}
