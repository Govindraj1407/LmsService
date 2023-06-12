using LMS.ViewModels;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;

namespace Services
{
    public interface ICourseWareService
    {
        public Task<string> CreateCourse(Course course);
        public Task<string> UpdateCourse(Course course);

        public Task<string> DeleteCourse(int courseId);

        public Task<IEnumerable<Course>> GetAllCourse();

        public Task<Course> GetCourse(int id);
    }
}
