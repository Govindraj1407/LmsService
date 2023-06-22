using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Models
{
    public class Course
    {
        public string CourseId { get; set; }

        [Required]
        public string CourseName { get; set; }

        [Required]
        public string Category { get; set; }

        public string CourseContent { get; set; }

        public string SubTopics { get; set; }

        public string Link { get; set; }

        public string UserId { get; set; }

        public string Status { get; set; }

        public string DueDate { get; set; }

        public int StudentCount { get; set; }
    }
}
