using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Models;
using Repository;

namespace Elms.Services
{
    public class CourseWareService : ICourseWareService
    {

        /// <summary>
        /// Gets or sets dynamo DB course repository
        /// </summary>
        private readonly ICourseWareDynamoDBRepository dynamoDBCourseRepository;

        /// <summary>
        /// Gets or sets user course service
        /// </summary>
        private readonly IUserCourseDynamoDBRepository dynamoDBUserCourseRepository;

        /// <summary>
        /// Gets or sets user course service
        /// </summary>
        //private readonly IUserCourseService userCourseService;

        public CourseWareService(ICourseWareDynamoDBRepository dynamoDBCourseRepository, IUserCourseDynamoDBRepository dynamoDBUserCourseRepository)
        {
            this.dynamoDBCourseRepository = dynamoDBCourseRepository;
            this.dynamoDBUserCourseRepository = dynamoDBUserCourseRepository;
            //this.userCourseService = userCourseService;
        }

        public async Task<Course> GetCourse(string id)
        {
            try
            {
                var course = await this.dynamoDBCourseRepository.GetCourse(id);
                return course;
            }
            catch (Exception ex)
            {
                throw new Exception("Error occured when get a course", ex);
            }

        }

        public async Task<IEnumerable<Course>> GetAllCourse()
        {
            try
            {
                var courses = await this.dynamoDBCourseRepository.GetAllCourse();
                var userCourses = await this.dynamoDBUserCourseRepository.GetAllUserCourses();
                var userCourseGroups = userCourses.GroupBy(n => n.CourseId)
                         .Select(n => new
                         {
                             courseId = n.Key,
                             StudentCount = n.Count()
                         });
                foreach (var course in courses)
                {
                    var userCourse = userCourseGroups.Where(x => x.courseId == course.CourseId);
                    if (userCourse.Any())
                    {
                        course.StudentCount = userCourse.FirstOrDefault().StudentCount;
                    }

                }
                return courses;
            }
            catch (Exception ex)
            {
                throw new Exception("Error occured when get all courses", ex);
            }
        }

        public async Task<string> CreateCourse(Course course)
        {
            try
            {
                course.CourseId = Guid.NewGuid().ToString();
                return await this.dynamoDBCourseRepository.CreateCourse(course);
            }
            catch (Exception exception)
            {
                throw new Exception("Course creation failed. Message:" + exception.Message);
            }

        }

        public async Task<string> UpdateCourse(Course course)
        {
            try
            {
                return await this.dynamoDBCourseRepository.UpdateCourse(course);
            }
            catch (Exception exception)
            {
                throw new Exception("Course update failed. Message:" + exception.Message);
            }

        }

        public async Task<string> DeleteCourse(string courseId)
        {
            try
            {
                return await this.dynamoDBCourseRepository.DeleteCourse(courseId);
            }
            catch (Exception exception)
            {
                throw new Exception("Course delete failed. Message:" + exception.Message);
            }

        }
    }
}
