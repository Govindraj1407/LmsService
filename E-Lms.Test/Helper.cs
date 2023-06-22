using Models;
using System;
using System.Collections.Generic;
using System.Text;
using ViewModels;

namespace Lms.Test
{
    public class Helper
    {
        public static IEnumerable<UserCourse> GetUserCourses()
        {
            return new List<UserCourse>()
            {
                new UserCourse()
                {
                    CourseId = "a3b49a36-2079-4c33-93d6-d88dec95f301",
                    UserId = "a3b49a36-3079-4c33-93d6-d88dec95f301",
                    Credits = 5,
                    Progress = 50,
                    UserCourseId = "a3b49a36-2079-4c33-93d6-d88dec95f302"                    
                },
                new UserCourse()
                {
                    CourseId = "b3b49a36-2079-4c33-93d6-d88dec95f301",
                    UserId = "a3b49a36-3079-4c33-93d6-d88dec95f301",
                    Credits = 5,
                    Progress = 50,
                    UserCourseId = "b3b49a36-2079-4c33-93d6-d88dec95f302"
                },
                new UserCourse()
                {
                    CourseId = "a3b49a36-2079-4c33-93d6-d88dec95f301",
                    UserId = "b3b49a36-3079-4c33-93d6-d88dec95f30b",
                    Credits = 5,
                    Progress = 50,
                    UserCourseId = "a3b49a36-2179-4c33-93d6-d88dec95f302"
                },
                new UserCourse()
                {
                    CourseId = "b3b49a36-2079-4c33-93d6-d88dec95f301",
                    UserId = "b3b49a36-3079-4c33-93d6-d88dec95f30b",
                    Credits = 5,
                    Progress = 50,
                    UserCourseId = "b3b49a36-2179-4c33-93d6-d88dec95f302"
                }
            };
        }

        public static IEnumerable<Course> GetAllCourses()
        {
            return new List<Course>()
            {
                new Course()
                {
                    CourseId = "a3b49a36-2079-4c33-93d6-d88dec95f301",
                    UserId = "a3b49a36-3079-4c33-93d6-d88dec95f301",
                    Category = "Devops"
                },
                new Course()
                {
                    CourseId = "b3b49a36-2079-4c33-93d6-d88dec95f301",
                    UserId = "a3b49a36-3079-4c33-93d6-d88dec95f301",
                    Category = "Devops"
                },
                new Course()
                {
                    CourseId = "a3b49a36-2079-4c33-93d6-d88dec95f301",
                    UserId = "b3b49a36-3079-4c33-93d6-d88dec95f30b",
                    Category = "Devops"
                },
                new Course()
                {
                    CourseId = "b3b49a36-2079-4c33-93d6-d88dec95f301",
                    UserId = "b3b49a36-3079-4c33-93d6-d88dec95f30b",
                   Category = "Devops"
                }
            };
        }

        public static IEnumerable<UserViewModel> GetAllUsers()
        {
            return new List<UserViewModel>()
            {
                new UserViewModel()
                {
                    UserId = "a3b49a36-3079-4c33-93d6-d88dec95f301",
                    Name = "Rishad"
                },
                new UserViewModel()
                {
                    UserId = "b3b49a36-3079-4c33-93d6-d88dec95f30b",
                    Name = "Sowmya"
                }
            };
        }
    }
}
