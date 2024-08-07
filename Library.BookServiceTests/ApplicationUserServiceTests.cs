using AutoMapper;
using Library.Core.Domain.Entities;
using Library.Core.Domain.IdentityEntities;
using Library.Core.Domain.RepositrotyContracts;
using Library.Core.DTO;
using Library.Core.DTO.Account;
using Library.Core.Services;
using Microsoft.AspNetCore.Identity;
using Moq;

namespace Library.ServicesTests
{
    public class ApplicationUserServiceTests
    {
        private readonly Mock<IUserBookViewRepository> _userViewRepositoryMock;
        private readonly Mock<IUserRepository> _userRepositoryMock;
        private readonly Mock<UserManager<ApplicationUser>> _userManagerMock;
        private readonly IMapper _mapper;
        private readonly ApplicationUserService _applicationUserService;

        public ApplicationUserServiceTests()
        {
            _userViewRepositoryMock = new Mock<IUserBookViewRepository>();
            _userRepositoryMock = new Mock<IUserRepository>();

            var store = new Mock<IUserStore<ApplicationUser>>();
            _userManagerMock = new Mock<UserManager<ApplicationUser>>(store.Object, null, null, null, null, null, null, null, null);

            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<ApplicationUser, ApplicationUserResponse>();
                cfg.CreateMap<ApplicationUserUpdateRequest, ApplicationUser>();
                cfg.CreateMap<UserBookView, UserBookViewResponse>();
            });
            _mapper = config.CreateMapper();

            _applicationUserService = new ApplicationUserService(
                _userViewRepositoryMock.Object,
                _userRepositoryMock.Object,
                _userManagerMock.Object,
                _mapper);
        }

        [Fact]
        public async Task GetAllAsync_Should_Return_All_Users()
        {
            // Arrange
            var users = new List<UserBookView>
            {
                new UserBookView { UserID = Guid.NewGuid(), BookID = Guid.NewGuid() },
                new UserBookView { UserID = Guid.NewGuid(), BookID = Guid.NewGuid() }
            };

            _userViewRepositoryMock.Setup(x => x.GetAllAsync())
                .ReturnsAsync(users);

            // Act
            var result = await _applicationUserService.GetAllAsync();

            // Assert
            Assert.NotNull(result);
            Assert.Equal(users.Count, result.Count());
        }

        [Fact]
        public async Task GetByIdAsync_Should_Return_User()
        {
            // Arrange
            var userId = Guid.NewGuid();
            var user = new ApplicationUser
            {
                Id = userId,
                UserName = "john.doe@example.com",
                Email = "john.doe@example.com"
            };

            _userRepositoryMock.Setup(x => x.GetByIdAsync(userId))
                .ReturnsAsync(user);

            // Act
            var result = await _applicationUserService.GetByIdAsync(userId);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(user.Email, result.User.Email);
        }

        [Fact]
        public async Task UpdateAsync_Should_Update_User()
        {
            // Arrange
            var userId = Guid.NewGuid();
            var updateRequest = new ApplicationUserUpdateRequest
            {
                UserId = userId,
                FirstName = "John",
                LastName = "Doe"
            };
            var user = new ApplicationUser
            {
                Id = userId,
                UserName = "john.doe@example.com",
                Email = "john.doe@example.com"
            };

            _userRepositoryMock.Setup(x => x.GetByIdAsync(userId))
                .ReturnsAsync(user);
            _userManagerMock.Setup(x => x.UpdateAsync(user))
                .ReturnsAsync(IdentityResult.Success);

            // Act
            var result = await _applicationUserService.UpdateAsync(updateRequest);

            // Assert
            Assert.True(result);
        }

        [Fact]
        public async Task DeleteByIdAsync_Should_Delete_User()
        {
            // Arrange
            var userId = Guid.NewGuid();
            _userViewRepositoryMock.Setup(x => x.DeleteAsync(userId))
                .ReturnsAsync(true);

            // Act
            var result = await _applicationUserService.DeleteByIdAsync(userId);

            // Assert
            Assert.True(result);
        }

        [Fact]
        public async Task UpdateNameAsync_Should_Update_User_Name()
        {
            // Arrange
            var userId = Guid.NewGuid();
            var firstName = "John";
            var lastName = "Doe";
            var user = new ApplicationUser
            {
                Id = userId,
                FirstName = "Jane",
                LastName = "Smith"
            };

            _userRepositoryMock.Setup(x => x.GetByIdAsync(userId))
                .ReturnsAsync(user);
            _userRepositoryMock.Setup(x => x.Update(user))
                .ReturnsAsync(true);

            // Act
            var result = await _applicationUserService.UpdateNameAsync(userId, firstName, lastName);

            // Assert
            Assert.True(result);
            Assert.Equal(firstName, user.FirstName);
            Assert.Equal(lastName, user.LastName);
        }
    }
}
