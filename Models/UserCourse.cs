using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Models
{
    public class UserCourse
    {
        public string UserCourseId { get; set; }

        public string UserId { get; set; }

        public string CourseId { get; set; }

        public int Progress { get; set; }

        public int Credits { get; set; }
    }
}
