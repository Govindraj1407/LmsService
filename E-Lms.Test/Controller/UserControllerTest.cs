namespace Lms.Test.Controller
{
    using AutoFixture;
    using Controllers;
    using Elms.Services;
    using Microsoft.AspNetCore.Mvc;
    using Models;
    using Moq;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Net;
    using System.Text;
    using System.Threading.Tasks;
    using ViewModels;
    using Xunit;

    public class UserControllerTest
    {
        /// <summary>
        /// User service mock
        /// </summary>
        private readonly Mock<IUserService> userServiceMock;

        /// <summary>
        /// User course service mock
        /// </summary>
        private readonly Mock<IUserCourseService> userCourseServiceMock;

        /// <summary>
        /// User controller
        /// </summary>
        private readonly UserController userController;

        private Fixture fixture;

        /// <summary>
        /// Initializes a new instance of the <see cref="UserControllerTest"/> class.
        /// User controller test
        /// </summary>
        public UserControllerTest()
        {
            this.userServiceMock = new Mock<IUserService>();
            this.userCourseServiceMock = new Mock<IUserCourseService>();
            this.userController = new UserController(this.userServiceMock.Object, this.userCourseServiceMock.Object);
            this.fixture = new Fixture();
        }
        
        [Fact]
        public async Task GetAll_NoData_ReturnsNoContent()
        {
            //Arrange
            this.userServiceMock.Setup(x => x.GetAllUser()).ReturnsAsync(() => null);

            // Act
            var actionResult = await this.userController.GetAll();

            // Assert
            Assert.Equal((int)HttpStatusCode.NoContent, ((NoContentResult)actionResult).StatusCode);
            this.userServiceMock.Verify(x => x.GetAllUser(), Times.Once);
        }

        [Fact]
        public async Task GetAll_ValidData_ReturnsUsers()
        {
            //Arrange
            IEnumerable<UserViewModel> users = this.fixture.CreateMany<UserViewModel>(5);
            this.userServiceMock.Setup(x => x.GetAllUser()).ReturnsAsync(users);

            // Act
            var actionResult = await this.userController.GetAll();

            // Assert
            Assert.Equal((int)HttpStatusCode.OK, ((OkObjectResult)actionResult).StatusCode);
            Assert.Equal(5, ((IEnumerable<UserViewModel>)((ObjectResult)actionResult).Value).Count());
            this.userServiceMock.Verify(x => x.GetAllUser(), Times.Once);
        }

        [Fact]
        public async Task GetUser_NoData_ReturnsNoContent()
        {
            //Arrange
            UserViewModel user = this.fixture.Create<UserViewModel>();
            this.userServiceMock.Setup(x => x.GetUser(It.IsAny<string>())).ReturnsAsync(() => null);

            // Act
            var actionResult = await this.userController.Get(user.UserId);

            // Assert
            Assert.Equal((int)HttpStatusCode.NoContent, ((NoContentResult)actionResult).StatusCode);
            this.userServiceMock.Verify(x => x.GetUser(user.UserId), Times.Once);
        }

        [Fact]
        public async Task GetUser_ValidData_ReturnsUser()
        {
            //Arrange
            UserViewModel user = this.fixture.Create<UserViewModel>();
            this.userServiceMock.Setup(x => x.GetUser(It.IsAny<string>())).ReturnsAsync(user);

            // Act
            var actionResult = await this.userController.Get(user.UserId);

            // Assert
            Assert.Equal((int)HttpStatusCode.OK, ((OkObjectResult)actionResult).StatusCode);
            Assert.Equal(user.Name, ((UserViewModel)((ObjectResult)actionResult).Value).Name);
            this.userServiceMock.Verify(x => x.GetUser(user.UserId), Times.Once);
        }

        [Fact]
        public async Task GetCourse_NoData_ReturnsNoContent()
        {
            //Arrange
            string userId = "379f8bf9-ab60-43e0-925d-f03af86b414f";
            IEnumerable<UserCourseViewModel> userCourses = this.fixture.CreateMany<UserCourseViewModel>();
            this.userCourseServiceMock.Setup(x => x.GetUserCourses(userId)).ReturnsAsync(() => null);

            // Act
            var actionResult = await this.userController.GetCourse(userId);

            // Assert
            Assert.Equal((int)HttpStatusCode.NoContent, ((NoContentResult)actionResult).StatusCode);
            this.userCourseServiceMock.Verify(x => x.GetUserCourses(userId), Times.Once);
        }

        [Fact]
        public async Task GetCourse_ValidData_ReturnsCourse()
        {
            //Arrange
            string userId = "379f8bf9-ab60-43e0-925d-f03af86b414f";
            IEnumerable<UserCourseViewModel> userCourses = this.fixture.CreateMany<UserCourseViewModel>();
            this.userCourseServiceMock.Setup(x => x.GetUserCourses(userId)).ReturnsAsync(userCourses);

            // Act
            var actionResult = await this.userController.GetCourse(userId);

            // Assert
            Assert.Equal((int)HttpStatusCode.OK, ((OkObjectResult)actionResult).StatusCode);
            Assert.Equal(userCourses.Count(), ((IEnumerable<UserCourseViewModel>)((ObjectResult)actionResult).Value).Count());
            this.userCourseServiceMock.Verify(x => x.GetUserCourses(userId), Times.Once);
        }

        [Fact]
        public async void Create_ValidData_ReturnsEmptyString()
        {
            // Arrange
            User user = this.fixture.Create<User>();
            this.userServiceMock.Setup(x => x.CreateUser(It.IsAny<User>())).ReturnsAsync(string.Empty);

            // Act
            var result = await this.userController.Create(user);

            // Assert
            Assert.Equal((int)HttpStatusCode.OK, ((OkResult)result).StatusCode);
            this.userServiceMock.Verify(x => x.CreateUser(It.IsAny<User>()), Times.Once);
        }

        [Fact]
        public async void Create_ValidData_ReturnsBadRequest()
        {
            // Arrange
            string message = "Exception while create user";
            User user = this.fixture.Create<User>();
            this.userServiceMock.Setup(x => x.CreateUser(It.IsAny<User>())).ReturnsAsync(message);

            // Act
            var result = await this.userController.Create(user);

            // Assert
            Assert.Equal((int)HttpStatusCode.BadRequest, ((BadRequestObjectResult)result).StatusCode);
            Assert.Equal(message, (((BadRequestObjectResult)result).Value).ToString());
            this.userServiceMock.Verify(x => x.CreateUser(It.IsAny<User>()), Times.Once);
        }

        [Fact]
        public async void Update_ValidData_ReturnsEmptyString()
        {
            // Arrange
            User user = this.fixture.Create<User>();
            this.userServiceMock.Setup(x => x.UpdateUser(It.IsAny<User>())).ReturnsAsync(string.Empty);

            // Act
            var result = await this.userController.Update(user);

            // Assert
            Assert.Equal((int)HttpStatusCode.OK, ((OkResult)result).StatusCode);
            this.userServiceMock.Verify(x => x.UpdateUser(It.IsAny<User>()), Times.Once);
        }

        [Fact]
        public async void Update_ValidData_ReturnsBadRequest()
        {
            // Arrange
            string message = "Exception while update user";
            User user = this.fixture.Create<User>();
            this.userServiceMock.Setup(x => x.UpdateUser(It.IsAny<User>())).ReturnsAsync(message);

            // Act
            var result = await this.userController.Update(user);

            // Assert
            Assert.Equal((int)HttpStatusCode.BadRequest, ((BadRequestObjectResult)result).StatusCode);
            Assert.Equal(message, (((BadRequestObjectResult)result).Value).ToString());
            this.userServiceMock.Verify(x => x.UpdateUser(It.IsAny<User>()), Times.Once);
        }

        [Fact]
        public async void Delete_ValidData_ReturnsEmptyString()
        {
            // Arrange
            string userId = "a3b49a36-2079-4c33-93d6-d88dec95f301";
            this.userServiceMock.Setup(x => x.DeleteUser(It.IsAny<string>())).ReturnsAsync(string.Empty);

            // Act
            var result = await this.userController.Delete(userId);

            // Assert
            Assert.Equal((int)HttpStatusCode.OK, ((OkResult)result).StatusCode);
            this.userServiceMock.Verify(x => x.DeleteUser(userId), Times.Once);
        }

        [Fact]
        public async void Delete_ValidData_ReturnsBadRequest()
        {
            // Arrange
            string message = "Exception while update user";
            string userId = "a3b49a36-2079-4c33-93d6-d88dec95f301";
            this.userServiceMock.Setup(x => x.DeleteUser(It.IsAny<string>())).ReturnsAsync(message);

            // Act
            var result = await this.userController.Delete(userId);

            // Assert
            Assert.Equal((int)HttpStatusCode.BadRequest, ((BadRequestObjectResult)result).StatusCode);
            Assert.Equal(message, (((BadRequestObjectResult)result).Value).ToString());
            this.userServiceMock.Verify(x => x.DeleteUser(userId), Times.Once);
        }
    }
}
