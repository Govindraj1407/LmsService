using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ViewModels
{
    public class UserCourseViewModel
    {
        public string UserCourseId { get; set; }

        public string UserId { get; set; }

        public string CourseId { get; set; }

        public string UserName { get; set; }

        public string CourseName { get; set; }

        public int Credits { get; set; }

        public int Progress { get; set; }

        public string Category { get; set; }

        public string CourseContent { get; set; }

        public string SubTopics { get; set; }

        public string Link { get; set; }

        public string Status { get; set; }

        public string DueDate { get; set; }

    }
}
