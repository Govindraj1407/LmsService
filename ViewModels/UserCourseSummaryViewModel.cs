using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ViewModels
{
    public class UserCourseSummaryViewModel
    {
        public string CourseId { get; set; }

        public int StudentCount { get; set; }

    }
}
