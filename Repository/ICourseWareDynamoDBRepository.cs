using Amazon.DynamoDBv2.DocumentModel;
using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Repository
{
    public interface ICourseWareDynamoDBRepository
    {
        public Task<Course> GetCourse(string id);

        public Task<IEnumerable<Course>> GetAllCourse();

        public Task<string> CreateCourse(Course course);

        public Task<string> UpdateCourse(Course course);

        public Task<string> DeleteCourse(string courseId);


    }
}
