using Models;
using System.Collections.Generic;
using System.Threading.Tasks;
using ViewModels;

namespace Elms.Services
{
    public interface IUserCourseService
    {
        public Task<IEnumerable<UserCourseViewModel>> GetUserCourses(string userId);

        public Task<string> AddUserCourse(IEnumerable<UserCourse> userCourses);

        public Task<IEnumerable<UserCourseSummaryViewModel>> GetUserCourseSummary();
    }
}
