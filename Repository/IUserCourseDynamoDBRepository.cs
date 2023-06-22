using Amazon.DynamoDBv2.DocumentModel;
using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Repository
{
    public interface IUserCourseDynamoDBRepository
    {
        public Task<IEnumerable<UserCourse>> GetUserCourses(string userId);
        public Task<string> AddUserCourse(UserCourse userCourse);

        public Task<IEnumerable<UserCourse>> GetAllUserCourses();
    }
}
