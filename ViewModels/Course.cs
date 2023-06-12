using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace LMS.ViewModels
{
    public class Course
    {
        public int CourseId { get; set; }
        public string CourseName { get; set; }

        public string CourseContent { get; set; }

        public string SubTopics { get; set; }

        public int UserId { get; set; }
    }
}
