namespace Lms.Test.Services
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Security.Claims;
    using System.Threading.Tasks;
    using AutoFixture;
    using AutoMapper;
    using Common;
    using Elms.Services;
    using Microsoft.AspNetCore.Http;
    using Models;
    using Moq;
    using Repository;
    using Xunit;

    public class UserServiceTest
    {
        /// <summary>
        /// Mapper
        /// </summary>
        private readonly IMapper mapper;

        /// <summary>
        /// Reason code repository mock
        /// </summary>
        private readonly Mock<IUserDynamoDBRepository> dynamoDBUserRepositoryMock;

        /// <summary>
        /// User service
        /// </summary>
        private readonly UserService userService;

        private Fixture fixture;

        public UserServiceTest()
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<AutoMapperProfile>();
            });
            this.mapper = config.CreateMapper();
            this.dynamoDBUserRepositoryMock = new Mock<IUserDynamoDBRepository>();
            this.userService = new UserService(this.dynamoDBUserRepositoryMock.Object, this.mapper);
            this.fixture = new Fixture();
        }

        [Fact]
        public async void GetUser_ValidData_ReturnsUser()
        {
            // Arrange
            User user = this.fixture.Create<User>();
            this.dynamoDBUserRepositoryMock.Setup(x => x.GetUser(It.IsAny<string>())).ReturnsAsync(user);
            string userId = user.UserId;

            // Act
            var result = await this.userService.GetUser(userId);

            // Assert
            Assert.Equal(result.Name, user.Name);
            Assert.Equal(result.Email, user.Email);
            this.dynamoDBUserRepositoryMock.Verify(x => x.GetUser(userId), Times.Once);
        }

        [Fact]
        public async void GetUser_InvalidData_ThrowsError()
        {
            // Arrange
            string exceptionMessage = "Dynamo DB exception";
            this.dynamoDBUserRepositoryMock.Setup(x => x.GetUser(It.IsAny<string>()))
                .Throws(new Exception(exceptionMessage));
            string userId = "a3b49a36-2079-4c33-93d6-d88dec95f301";

            // Act
            Exception result = await Assert.ThrowsAsync<Exception>(
                async () => await this.userService.GetUser(userId));

            // Assert
            Assert.Equal(exceptionMessage, result.InnerException.Message);
            this.dynamoDBUserRepositoryMock.Verify(x => x.GetUser(userId), Times.Once);
        }

        [Fact]
        public async void GetAllUser_ValidData_ReturnsAllUser()
        {
            // Arrange
            IEnumerable<User> users = this.fixture.CreateMany<User>(10);
            this.dynamoDBUserRepositoryMock.Setup(x => x.GetAllUser()).ReturnsAsync(users);

            // Act
            var result = await this.userService.GetAllUser();

            // Assert
            Assert.Equal(10, result.Count());
            this.dynamoDBUserRepositoryMock.Verify(x => x.GetAllUser(), Times.Once);
        }

        [Fact]
        public async void GetAllUser_InvalidData_ThrowsError()
        {
            // Arrange
            User user = this.fixture.Create<User>();
            string exceptionMessage = "Dynamo DB exception";
            this.dynamoDBUserRepositoryMock.Setup(x => x.GetAllUser())
                .Throws(new Exception(exceptionMessage));

            // Act
            Exception result = await Assert.ThrowsAsync<Exception>(
                async () => await this.userService.GetAllUser());

            // Assert
            Assert.Equal(exceptionMessage, result.InnerException.Message);
            this.dynamoDBUserRepositoryMock.Verify(x => x.GetAllUser(), Times.Once);
        }

        [Fact]
        public async void CreateUser_ValidData_ReturnsEmptyString()
        {
            // Arrange
            User user = this.fixture.Create<User>();
            this.dynamoDBUserRepositoryMock.Setup(x => x.CreateUser(It.IsAny<User>())).ReturnsAsync(string.Empty);

            // Act
            var result = await this.userService.CreateUser(user);

            // Assert
            Assert.Equal(string.Empty, result);
            this.dynamoDBUserRepositoryMock.Verify(x => x.CreateUser(user), Times.Once);
        }

        [Fact]
        public async void CreateUser_InvalidData_ThrowsException()
        {
            // Arrange
            User user = this.fixture.Create<User>();
            string exceptionMessage = "Dynamo DB exception";
            this.dynamoDBUserRepositoryMock.Setup(x => x.CreateUser(It.IsAny<User>()))
                .Throws(new Exception(exceptionMessage));

            // Act
            Exception result = await Assert.ThrowsAsync<Exception>(
                async () => await this.userService.CreateUser(user));

            // Assert
            Assert.Equal(exceptionMessage, result.InnerException.Message);
            this.dynamoDBUserRepositoryMock.Verify(x => x.CreateUser(user), Times.Once);
        }

        [Fact]
        public async void UpdateUser_ValidData_ReturnsEmptyString()
        {
            // Arrange
            User user = this.fixture.Create<User>();
            this.dynamoDBUserRepositoryMock.Setup(x => x.UpdateUser(It.IsAny<User>())).ReturnsAsync(string.Empty);

            // Act
            var result = await this.userService.UpdateUser(user);

            // Assert
            Assert.Equal(string.Empty, result);
            this.dynamoDBUserRepositoryMock.Verify(x => x.UpdateUser(user), Times.Once);
        }

        [Fact]
        public async void UpdateUser_InvalidData_ThrowsException()
        {
            // Arrange
            User user = this.fixture.Create<User>();
            string exceptionMessage = "Dynamo DB exception";
            this.dynamoDBUserRepositoryMock.Setup(x => x.UpdateUser(It.IsAny<User>()))
                .Throws(new Exception(exceptionMessage));

            // Act
            Exception result = await Assert.ThrowsAsync<Exception>(
                async () => await this.userService.UpdateUser(user));

            // Assert
            Assert.Equal(exceptionMessage, result.InnerException.Message);
            this.dynamoDBUserRepositoryMock.Verify(x => x.UpdateUser(user), Times.Once);
        }

        [Fact]
        public async void DeleteUser_ValidData_ReturnsEmptyString()
        {
            // Arrange
            string userId = "a3b49a36-2079-4c33-93d6-d88dec95f301";
            this.dynamoDBUserRepositoryMock.Setup(x => x.DeleteUser(It.IsAny<string>())).ReturnsAsync(string.Empty);

            // Act
            var result = await this.userService.DeleteUser(userId);

            // Assert
            Assert.Equal(string.Empty, result);
            this.dynamoDBUserRepositoryMock.Verify(x => x.DeleteUser(userId), Times.Once);
        }

        [Fact]
        public async void DeleteUser_InvalidData_ThrowsException()
        {
            // Arrange
            string userId = "a3b49a36-2079-4c33-93d6-d88dec95f301";
            string exceptionMessage = "Dynamo DB exception";
            this.dynamoDBUserRepositoryMock.Setup(x => x.DeleteUser(It.IsAny<string>()))
                .Throws(new Exception(exceptionMessage));

            // Act
            Exception result = await Assert.ThrowsAsync<Exception>(
                async () => await this.userService.DeleteUser(userId));

            // Assert
            Assert.Equal(exceptionMessage, result.InnerException.Message);
            this.dynamoDBUserRepositoryMock.Verify(x => x.DeleteUser(userId), Times.Once);
        }
    }
}
