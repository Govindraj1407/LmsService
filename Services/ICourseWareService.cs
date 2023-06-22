using Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Elms.Services
{
    public interface ICourseWareService
    {
        public Task<string> CreateCourse(Course course);
        public Task<string> UpdateCourse(Course course);

        public Task<string> DeleteCourse(string courseId);

        public Task<IEnumerable<Course>> GetAllCourse();

        public Task<Course> GetCourse(string id);
    }
}
