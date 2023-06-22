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

    public class CourseControllerTest
    {
        /// <summary>
        /// Course service mock
        /// </summary>
        private readonly Mock<ICourseWareService> courseServiceMock;

        /// <summary>
        /// Course controller
        /// </summary>
        private readonly CourseWareController courseController;

        private Fixture fixture;

        /// <summary>
        /// Initializes a new instance of the <see cref="courseControllerTest"/> class.
        /// Course controller test
        /// </summary>
        public CourseControllerTest()
        {
            this.courseServiceMock = new Mock<ICourseWareService>();
            this.courseController = new CourseWareController(this.courseServiceMock.Object);
            this.fixture = new Fixture();
        }
        
        [Fact]
        public async Task GetAll_NoData_ReturnsNoContent()
        {
            //Arrange
            this.courseServiceMock.Setup(x => x.GetAllCourse()).ReturnsAsync(() => null);

            // Act
            var actionResult = await this.courseController.GetAll();

            // Assert
            Assert.Equal((int)HttpStatusCode.NoContent, ((NoContentResult)actionResult).StatusCode);
            this.courseServiceMock.Verify(x => x.GetAllCourse(), Times.Once);
        }

        [Fact]
        public async Task GetAll_ValidData_ReturnsUsers()
        {
            //Arrange
            IEnumerable<Course> courses = this.fixture.CreateMany<Course>(5);
            this.courseServiceMock.Setup(x => x.GetAllCourse()).ReturnsAsync(courses);

            // Act
            var actionResult = await this.courseController.GetAll();

            // Assert
            Assert.Equal((int)HttpStatusCode.OK, ((OkObjectResult)actionResult).StatusCode);
            Assert.Equal(5, ((IEnumerable<Course>)((ObjectResult)actionResult).Value).Count());
            this.courseServiceMock.Verify(x => x.GetAllCourse(), Times.Once);
        }

        [Fact]
        public async Task GetCourse_NoData_ReturnsNoContent()
        {
            //Arrange
            Course course = this.fixture.Create<Course>();
            this.courseServiceMock.Setup(x => x.GetCourse(course.CourseId)).ReturnsAsync(() => null);

            // Act
            var actionResult = await this.courseController.Get(course.CourseId);

            // Assert
            Assert.Equal((int)HttpStatusCode.NoContent, ((NoContentResult)actionResult).StatusCode);
            this.courseServiceMock.Verify(x => x.GetCourse(course.CourseId), Times.Once);
        }

        [Fact]
        public async Task GetCourse_ValidData_ReturnsCourse()
        {
            //Arrange
            Course course = this.fixture.Create<Course>();
            this.courseServiceMock.Setup(x => x.GetCourse(course.CourseId)).ReturnsAsync(course);

            // Act
            var actionResult = await this.courseController.Get(course.CourseId);

            // Assert
            Assert.Equal((int)HttpStatusCode.OK, ((OkObjectResult)actionResult).StatusCode);
            Assert.Equal(course.CourseName, ((Course)((ObjectResult)actionResult).Value).CourseName);
            this.courseServiceMock.Verify(x => x.GetCourse(course.CourseId), Times.Once);
        }

        [Fact]
        public async void Create_ValidData_ReturnsEmptyString()
        {
            // Arrange
            Course course = this.fixture.Create<Course>();
            this.courseServiceMock.Setup(x => x.CreateCourse(It.IsAny<Course>())).ReturnsAsync(string.Empty);

            // Act
            var result = await this.courseController.Create(course);

            // Assert
            Assert.Equal((int)HttpStatusCode.OK, ((OkResult)result).StatusCode);
            this.courseServiceMock.Verify(x => x.CreateCourse(It.IsAny<Course>()), Times.Once);
        }

        [Fact]
        public async void Create_ValidData_ReturnsBadRequest()
        {
            // Arrange
            string message = "Exception while create course";
            Course course = this.fixture.Create<Course>();
            this.courseServiceMock.Setup(x => x.CreateCourse(It.IsAny<Course>())).ReturnsAsync(message);

            // Act
            var result = await this.courseController.Create(course);

            // Assert
            Assert.Equal((int)HttpStatusCode.BadRequest, ((BadRequestObjectResult)result).StatusCode);
            Assert.Equal(message, (((BadRequestObjectResult)result).Value).ToString());
            this.courseServiceMock.Verify(x => x.CreateCourse(It.IsAny<Course>()), Times.Once);
        }

        [Fact]
        public async void Update_ValidData_ReturnsEmptyString()
        {
            // Arrange
            Course course = this.fixture.Create<Course>();
            this.courseServiceMock.Setup(x => x.UpdateCourse(It.IsAny<Course>())).ReturnsAsync(string.Empty);

            // Act
            var result = await this.courseController.Update(course);

            // Assert
            Assert.Equal((int)HttpStatusCode.OK, ((OkResult)result).StatusCode);
            this.courseServiceMock.Verify(x => x.UpdateCourse(It.IsAny<Course>()), Times.Once);
        }

        [Fact]
        public async void Update_ValidData_ReturnsBadRequest()
        {
            // Arrange
            string message = "Exception while update course";
            Course course = this.fixture.Create<Course>();
            this.courseServiceMock.Setup(x => x.UpdateCourse(It.IsAny<Course>())).ReturnsAsync(message);

            // Act
            var result = await this.courseController.Update(course);

            // Assert
            Assert.Equal((int)HttpStatusCode.BadRequest, ((BadRequestObjectResult)result).StatusCode);
            Assert.Equal(message, (((BadRequestObjectResult)result).Value).ToString());
            this.courseServiceMock.Verify(x => x.UpdateCourse(It.IsAny<Course>()), Times.Once);
        }

        [Fact]
        public async void Delete_ValidData_ReturnsEmptyString()
        {
            // Arrange
            string courseId = "a3b49a36-2079-4c33-93d6-d88dec95f301";
            this.courseServiceMock.Setup(x => x.DeleteCourse(It.IsAny<string>())).ReturnsAsync(string.Empty);

            // Act
            var result = await this.courseController.Delete(courseId);

            // Assert
            Assert.Equal((int)HttpStatusCode.OK, ((OkResult)result).StatusCode);
            this.courseServiceMock.Verify(x => x.DeleteCourse(courseId), Times.Once);
        }

        [Fact]
        public async void Delete_ValidData_ReturnsBadRequest()
        {
            // Arrange
            string message = "Exception while update user";
            string courseId = "a3b49a36-2079-4c33-93d6-d88dec95f301";
            this.courseServiceMock.Setup(x => x.DeleteCourse(courseId)).ReturnsAsync(message);

            // Act
            var result = await this.courseController.Delete(courseId);

            // Assert
            Assert.Equal((int)HttpStatusCode.BadRequest, ((BadRequestObjectResult)result).StatusCode);
            Assert.Equal(message, (((BadRequestObjectResult)result).Value).ToString());
            this.courseServiceMock.Verify(x => x.DeleteCourse(courseId), Times.Once);
        }
    }
}
