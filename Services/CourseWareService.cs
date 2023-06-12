using Core.Repository;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using DynamoDBWrapper;
using LMS.ViewModels;
using Amazon.DynamoDBv2.DataModel;
using Amazon.DynamoDBv2.Model;

namespace Services
{
    public class CourseWareService : ICourseWareService
    {
        ///// <summary>
        ///// Pricing document db repository
        ///// </summary>
        //private readonly IUserDocumentDbRepository userDocumentDbRepository;

        /// <summary>
        /// Gets or sets dynamo DB repository
        /// </summary>
        private readonly IDynamoDBRepository<int, Course> dynamoDBRepository;

        public CourseWareService(IDynamoRepositoryFactory factory)
        {
            this.dynamoDBRepository = factory.Get<int, Course>(
                "Course",
                "CourseId");
        }

        public async Task<Course> GetCourse(int id)
        {
            try
            {
                var attributes = new List<string>() { "CourseId", "CourseName", "CourseContent", "SubTopics", "UserId" };

                var course = await this.dynamoDBRepository.GetAsync(id);
                return course;
            }
            catch(Exception ex)
            {
                throw new Exception("Error occured when get course", ex);
            }
            
        }

        public async Task<IEnumerable<Course>> GetAllCourse()
        {
            try
            {
                var conditions = new List<ScanFilterCondition>();
                var courses = await this.dynamoDBRepository.ScanAsync(conditions);
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
                await this.dynamoDBRepository.InsertAsync(course);
                return string.Empty;
            }
            catch(Exception exception)
            {
                return "Course creation failed. Message:"+ exception.Message;
            }
            
        }

        public async Task<string> UpdateCourse(Course course)
        {
            try
            {
                await this.dynamoDBRepository.PartialUpdateCommandAsync(course);
                return string.Empty;
            }
            catch (Exception exception)
            {
                return "Course update failed. Message:" + exception.Message;
            }

        }

        public async Task<string> DeleteCourse(int courseId)
        {
            try
            {
                await this.dynamoDBRepository.DeleteAsync(courseId);
                return string.Empty;
            }
            catch (Exception exception)
            {
                return "Course update failed. Message:" + exception.Message;
            }

        }
    }
}
