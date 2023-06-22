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

    public class CourseWareServiceTest
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
        private readonly CourseWareService courseWareService;

        private Fixture fixture;

        /// <summary>
        /// Initializes a new instance of the <see cref="CourseWareServiceTest"/> class.
        /// Course ware service test
        /// </summary>
        public CourseWareServiceTest()
        {
            this.dynamoDBCourseRepositoryMock = new Mock<ICourseWareDynamoDBRepository>();
            this.dynamoDBUserCourseRepositoryMock = new Mock<IUserCourseDynamoDBRepository>();
            this.courseWareService = new CourseWareService(this.dynamoDBCourseRepositoryMock.Object, this.dynamoDBUserCourseRepositoryMock.Object);
            this.fixture = new Fixture();
        }

        [Fact]
        public async void GetCourse_ValidData_ReturnsCourse()
        {
            // Arrange
            Course course = this.fixture.Create<Course>();
            this.dynamoDBCourseRepositoryMock.Setup(x => x.GetCourse(It.IsAny<string>())).ReturnsAsync(course);
            string courseId = course.CourseId;

            // Act
            var result = await this.courseWareService.GetCourse(courseId);

            // Assert
            Assert.Equal(result.CourseName, course.CourseName);
            Assert.Equal(result.Category, course.Category);
            this.dynamoDBCourseRepositoryMock.Verify(x => x.GetCourse(courseId), Times.Once);
        }

        [Fact]
        public async void GetCourse_InvalidData_ThrowsException()
        {
            // Arrange
            string exceptionMessage = "Dynamo DB exception";
            this.dynamoDBCourseRepositoryMock.Setup(x => x.GetCourse(It.IsAny<string>()))
                .Throws(new Exception(exceptionMessage));
            string courseId = "a3b49a36-2079-4c33-93d6-d88dec95f301";

            // Act
            Exception result = await Assert.ThrowsAsync<Exception>(
                async () => await this.courseWareService.GetCourse(courseId));

            // Assert
            Assert.Equal(exceptionMessage, result.InnerException.Message);
            this.dynamoDBCourseRepositoryMock.Verify(x => x.GetCourse(courseId), Times.Once);
        }

        [Fact]
        public async void GetAllCourse_ValidData_ReturnsAllCourse()
        {
            // Arrange
            IEnumerable<Course> courses = this.fixture.CreateMany<Course>(10);
            this.dynamoDBCourseRepositoryMock.Setup(x => x.GetAllCourse()).ReturnsAsync(courses);

            // Act
            var result = await this.courseWareService.GetAllCourse();

            // Assert
            Assert.Equal(10, result.Count());
            this.dynamoDBCourseRepositoryMock.Verify(x => x.GetAllCourse(), Times.Once);
        }

        [Fact]
        public async void GetAllCourse_InvalidData_ThrowsException()
        {
            // Arrange
            Course user = this.fixture.Create<Course>();
            string exceptionMessage = "Dynamo DB exception";
            this.dynamoDBCourseRepositoryMock.Setup(x => x.GetAllCourse())
                .Throws(new Exception(exceptionMessage));

            // Act
            Exception result = await Assert.ThrowsAsync<Exception>(
                async () => await this.courseWareService.GetAllCourse());

            // Assert
            Assert.Equal(exceptionMessage, result.InnerException.Message);
            this.dynamoDBCourseRepositoryMock.Verify(x => x.GetAllCourse(), Times.Once);
        }

        [Fact]
        public async void CreateCourse_ValidData_ReturnsEmptyString()
        {
            // Arrange
            Course course = this.fixture.Create<Course>();
            this.dynamoDBCourseRepositoryMock.Setup(x => x.CreateCourse(It.IsAny<Course>())).ReturnsAsync(string.Empty);

            // Act
            var result = await this.courseWareService.CreateCourse(course);

            // Assert
            Assert.Equal(string.Empty, result);
            this.dynamoDBCourseRepositoryMock.Verify(x => x.CreateCourse(course), Times.Once);
        }

        [Fact]
        public async void CreateCourse_InvalidData_ThrowsException()
        {
            // Arrange
            Course course = this.fixture.Create<Course>();
            string exceptionMessage = "Dynamo DB exception";
            this.dynamoDBCourseRepositoryMock.Setup(x => x.CreateCourse(It.IsAny<Course>()))
                .Throws(new Exception(exceptionMessage));

            // Act
            Exception result = await Assert.ThrowsAsync<Exception>(
                async () => await this.courseWareService.CreateCourse(course));

            // Assert
            Assert.Equal("Course creation failed. Message:" + exceptionMessage, result.Message);
            this.dynamoDBCourseRepositoryMock.Verify(x => x.CreateCourse(course), Times.Once);
        }

        [Fact]
        public async void UpdateCourse_ValidData_ReturnsEmptyString()
        {
            // Arrange
            Course course = this.fixture.Create<Course>();
            this.dynamoDBCourseRepositoryMock.Setup(x => x.UpdateCourse(It.IsAny<Course>())).ReturnsAsync(string.Empty);

            // Act
            var result = await this.courseWareService.UpdateCourse(course);

            // Assert
            Assert.Equal(string.Empty, result);
            this.dynamoDBCourseRepositoryMock.Verify(x => x.UpdateCourse(course), Times.Once);
        }

        [Fact]
        public async void UpdateCourse_InvalidData_ThrowsException()
        {
            // Arrange
            Course course = this.fixture.Create<Course>();
            string exceptionMessage = "Dynamo DB exception";
            this.dynamoDBCourseRepositoryMock.Setup(x => x.UpdateCourse(It.IsAny<Course>()))
                .Throws(new Exception(exceptionMessage));

            // Act
            Exception result = await Assert.ThrowsAsync<Exception>(
                async () => await this.courseWareService.UpdateCourse(course));

            // Assert
            Assert.Equal("Course update failed. Message:" + exceptionMessage, result.Message);
            this.dynamoDBCourseRepositoryMock.Verify(x => x.UpdateCourse(course), Times.Once);
        }

        [Fact]
        public async void DeleteCourse_ValidData_ReturnsEmptyString()
        {
            // Arrange
            string courseId = "a3b49a36-2079-4c33-93d6-d88dec95f301";
            this.dynamoDBCourseRepositoryMock.Setup(x => x.DeleteCourse(It.IsAny<string>())).ReturnsAsync(string.Empty);

            // Act
            var result = await this.courseWareService.DeleteCourse(courseId);

            // Assert
            Assert.Equal(string.Empty, result);
            this.dynamoDBCourseRepositoryMock.Verify(x => x.DeleteCourse(courseId), Times.Once);
        }

        [Fact]
        public async void DeleteCourse_InvalidData_ThrowsException()
        {
            // Arrange
            string courseId = "a3b49a36-2079-4c33-93d6-d88dec95f301";
            string exceptionMessage = "Dynamo DB exception";
            this.dynamoDBCourseRepositoryMock.Setup(x => x.DeleteCourse(It.IsAny<string>()))
                .Throws(new Exception(exceptionMessage));

            // Act
            Exception result = await Assert.ThrowsAsync<Exception>(
                async () => await this.courseWareService.DeleteCourse(courseId));

            // Assert
            Assert.Equal("Course delete failed. Message:" + exceptionMessage, result.Message);
            this.dynamoDBCourseRepositoryMock.Verify(x => x.DeleteCourse(courseId), Times.Once);
        }
    }
}
