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

    public class UserCourseControllerTest
    {
        /// <summary>
        /// User course service mock
        /// </summary>
        private readonly Mock<IUserCourseService> userCourseServiceMock;

        /// <summary>
        /// Course controller
        /// </summary>
        private readonly UserCourseController userCourseController;

        private Fixture fixture;

        /// <summary>
        /// Initializes a new instance of the <see cref="UserCourseControllerTest"/> class.
        /// User course controller test
        /// </summary>
        public UserCourseControllerTest()
        {
            this.userCourseServiceMock = new Mock<IUserCourseService>();
            this.userCourseController = new UserCourseController(this.userCourseServiceMock.Object);
            this.fixture = new Fixture();
        }

        [Fact]
        public async void Create_ValidData_ReturnsEmptyString()
        {
            // Arrange
            IEnumerable<UserCourse> userCourses = this.fixture.CreateMany<UserCourse>(5);
            this.userCourseServiceMock.Setup(x => x.AddUserCourse(It.IsAny<IEnumerable<UserCourse>>())).ReturnsAsync(string.Empty);

            // Act
            var result = await this.userCourseController.Create(userCourses);

            // Assert
            Assert.Equal((int)HttpStatusCode.OK, ((OkResult)result).StatusCode);
            this.userCourseServiceMock.Verify(x => x.AddUserCourse(userCourses), Times.Once);
        }

        [Fact]
        public async void Create_ValidData_ReturnsBadRequest()
        {
            // Arrange
            string message = "Exception while create course";
            IEnumerable<UserCourse> userCourses = this.fixture.CreateMany<UserCourse>();
            this.userCourseServiceMock.Setup(x => x.AddUserCourse(It.IsAny<IEnumerable<UserCourse>>())).ReturnsAsync(message);

            // Act
            var result = await this.userCourseController.Create(userCourses);

            // Assert
            Assert.Equal((int)HttpStatusCode.BadRequest, ((BadRequestObjectResult)result).StatusCode);
            Assert.Equal(message, (((BadRequestObjectResult)result).Value).ToString());
            this.userCourseServiceMock.Verify(x => x.AddUserCourse(userCourses), Times.Once);
        }
    }
}
