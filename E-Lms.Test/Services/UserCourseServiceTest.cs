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

    public class UserCourseServiceTest
    {

        /// <summary>
        /// Course repository mock
        /// </summary>
        private readonly Mock<ICourseWareDynamoDBRepository> dynamoDBCourseRepositoryMock;

        /// <summary>
        /// User course repository mock
        /// </summary>
        private readonly Mock<IUserCourseDynamoDBRepository> dynamoDBUserCourseRepositoryMock;

        /// <summary>
        /// Course ware service
        /// </summary>
        private readonly Mock<ICourseWareService> courseWareServiceMock;

        /// <summary>
        /// User course service
        /// </summary>
        private readonly UserCourseService userCourseService;

        /// <summary>
        /// User service
        /// </summary>
        private readonly Mock<IUserService> userServiceMock;

        private Fixture fixture;

        /// <summary>
        /// Initializes a new instance of the <see cref="UserCourseServiceTest"/> class.
        /// Course ware service test
        /// </summary>
        public UserCourseServiceTest()
        {
            this.dynamoDBUserCourseRepositoryMock = new Mock<IUserCourseDynamoDBRepository>();
            this.dynamoDBCourseRepositoryMock = new Mock<ICourseWareDynamoDBRepository>();
            this.userServiceMock = new Mock<IUserService>();
            this.courseWareServiceMock = new Mock<ICourseWareService>();
            this.userCourseService = new UserCourseService(this.dynamoDBUserCourseRepositoryMock.Object, this.courseWareServiceMock.Object, this.userServiceMock.Object);
            this.fixture = new Fixture();
        }

        [Fact]
        public async void GetUserCourses_ValidData_ReturnsUserCourses()
        {
            // Arrange
            this.dynamoDBUserCourseRepositoryMock.Setup(x => x.GetUserCourses(It.IsAny<string>())).ReturnsAsync(Helper.GetUserCourses());
            this.courseWareServiceMock.Setup(x => x.GetAllCourse()).ReturnsAsync(Helper.GetAllCourses());
            this.userServiceMock.Setup(x => x.GetAllUser()).ReturnsAsync(Helper.GetAllUsers());
            string userId = "a3b49a36-2079-4c33-93d6-d88dec95f301";

            // Act
            var result = await this.userCourseService.GetUserCourses(userId);

            // Assert
            Assert.Equal(8, result.Count());
            this.dynamoDBUserCourseRepositoryMock.Verify(x => x.GetUserCourses(userId), Times.Once);
            this.courseWareServiceMock.Verify(x => x.GetAllCourse(), Times.Once);
            this.userServiceMock.Verify(x => x.GetAllUser(), Times.Once);
        }

        [Fact]
        public async void GetUserCourses_InvalidData_ThrowsException()
        {
            // Arrange
            string exceptionMessage = "Dynamo DB exception";
            this.dynamoDBUserCourseRepositoryMock.Setup(x => x.GetUserCourses(It.IsAny<string>()))
                .Throws(new Exception(exceptionMessage));
            string userId = "a3b49a36-2079-4c33-93d6-d88dec95f301";

            // Act
            Exception result = await Assert.ThrowsAsync<Exception>(
                async () => await this.userCourseService.GetUserCourses(userId));

            // Assert
            Assert.Equal(exceptionMessage, result.InnerException.Message);
            this.dynamoDBUserCourseRepositoryMock.Verify(x => x.GetUserCourses(userId), Times.Once);
            this.courseWareServiceMock.Verify(x => x.GetAllCourse(), Times.Never);
            this.userServiceMock.Verify(x => x.GetAllUser(), Times.Never);
        }

        [Fact]
        public async void AddUserCourse_ValidData_ReturnsEmpty()
        {
            // Arrange
            IEnumerable<UserCourse> userCourse = this.fixture.CreateMany<UserCourse>(5);

            // Act
            var result = await this.userCourseService.AddUserCourse(userCourse);

            // Assert
            Assert.Equal(string.Empty, result);
            this.dynamoDBUserCourseRepositoryMock.Verify(x => x.AddUserCourse(It.IsAny<UserCourse>()), Times.Exactly(5));
        }

        [Fact]
        public async void AddUserCourse_InvalidData_ThrowsException()
        {
            // Arrange
            string exceptionMessage = "Dynamo DB exception";
            IEnumerable<UserCourse> userCourses = this.fixture.CreateMany<UserCourse>(5);
            this.dynamoDBUserCourseRepositoryMock.Setup(x => x.AddUserCourse(It.IsAny<UserCourse>()))
                .Throws(new Exception(exceptionMessage));

            // Act
            Exception result = await Assert.ThrowsAsync<Exception>(
                async () => await this.userCourseService.AddUserCourse(userCourses));

            // Assert
            Assert.Equal(exceptionMessage, result.InnerException.Message);
            this.dynamoDBUserCourseRepositoryMock.Verify(x => x.AddUserCourse(It.IsAny<UserCourse>()), Times.Once);
        }

        [Fact]
        public async void GetUserCourseSummary_ValidData_ReturnsUserCourseSummary()
        {
            // Arrange
            this.dynamoDBUserCourseRepositoryMock.Setup(x => x.GetAllUserCourses()).ReturnsAsync(Helper.GetUserCourses());
            this.courseWareServiceMock.Setup(x => x.GetAllCourse()).ReturnsAsync(Helper.GetAllCourses());
            this.userServiceMock.Setup(x => x.GetAllUser()).ReturnsAsync(Helper.GetAllUsers());

            // Act
            var result = await this.userCourseService.GetUserCourseSummary();

            // Assert
            Assert.Equal(2, result.ElementAt(1).StudentCount);
            this.dynamoDBUserCourseRepositoryMock.Verify(x => x.GetAllUserCourses(), Times.Once);
        }

        [Fact]
        public async void GetUserCourseSummary_InvalidData_ThrowsException()
        {
            // Arrange
            string exceptionMessage = "Dynamo DB exception";
            this.dynamoDBUserCourseRepositoryMock.Setup(x => x.GetAllUserCourses())
                .Throws(new Exception(exceptionMessage));

            // Act
            Exception result = await Assert.ThrowsAsync<Exception>(
                async () => await this.userCourseService.GetUserCourseSummary());

            // Assert
            Assert.Equal(exceptionMessage, result.InnerException.Message);
            this.dynamoDBUserCourseRepositoryMock.Verify(x => x.GetAllUserCourses(), Times.Once);
        }

    }
}
