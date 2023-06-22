using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using AutoMapper;
using DynamoDBWrapper;
using Models;
using Repository;
using ViewModels;

namespace Elms.Services
{
    public class UserCourseService : IUserCourseService
    {

        /// <summary>
        /// Gets or sets dynamo DB user course repository
        /// </summary>
        private readonly IUserCourseDynamoDBRepository dynamoDBUserCourseRepository;

        /// <summary>
        /// Gets or sets course service
        /// </summary>
        private readonly ICourseWareService courseWareService;

        /// <summary>
        /// Gets or sets user service
        /// </summary>
        private readonly IUserService userService;

        public UserCourseService(IUserCourseDynamoDBRepository dynamoDBUserCourseRepository, ICourseWareService courseWareService, IUserService userService)
        {
            this.dynamoDBUserCourseRepository = dynamoDBUserCourseRepository;
            this.courseWareService = courseWareService;
            this.userService = userService;
        }

        public async Task<IEnumerable<UserCourseSummaryViewModel>> GetUserCourseSummary()
        {
            try
            {
                var userCourses = await this.dynamoDBUserCourseRepository.GetAllUserCourses();
                IList<UserCourseSummaryViewModel> usercourseSummaryViewModel = new List<UserCourseSummaryViewModel>();
                var userCourseGroups = userCourses.GroupBy(n => n.CourseId)
                         .Select(n => new
                         {
                             courseId = n.Key,
                             StudentCount = n.Count()
                         });
                foreach (var userCourseGroup in userCourseGroups)
                {
                    UserCourseSummaryViewModel userCourseSummary = new UserCourseSummaryViewModel();
                    userCourseSummary.CourseId = userCourseGroup.courseId;
                    userCourseSummary.StudentCount = userCourseGroup.StudentCount;
                    usercourseSummaryViewModel.Add(userCourseSummary);
                }
                return usercourseSummaryViewModel;
            }
            catch (Exception ex)
            {
                throw new Exception("Error occured when get a user courses", ex);
            }
        }

        public async Task<IEnumerable<UserCourseViewModel>> GetUserCourses(string userId)
        {
            try
            {
                var userCourses = await this.dynamoDBUserCourseRepository.GetUserCourses(userId);
                var courses = await this.courseWareService.GetAllCourse();
                var users = await this.userService.GetAllUser();
                var userCourseLinq = from a in userCourses
                        join b in courses on a.CourseId equals b.CourseId
                        select new
                        {
                            a.UserId,
                            a.Progress,
                            a.Credits,
                            a.CourseId,
                            b.CourseName,
                            b.Category,
                            b.CourseContent,
                            b.Link,
                            b.SubTopics,
                            b.Status,
                            b.DueDate
                        };
                IList<UserCourseViewModel> usercoursesViewModel = new List<UserCourseViewModel>();
                foreach (var userCourse in userCourseLinq)
                {
                    var user = users.Where(x => x.UserId == userCourse.UserId);                   
                    UserCourseViewModel usercourseViewModel = new UserCourseViewModel();
                    if (user.Any())
                    {
                        usercourseViewModel.UserName = user.FirstOrDefault().Name;
                    }
                    usercourseViewModel.Category = userCourse.Category;
                    usercourseViewModel.UserId = userCourse.UserId;
                    usercourseViewModel.Progress = userCourse.Progress;
                    usercourseViewModel.Credits = userCourse.Credits;
                    usercourseViewModel.CourseId = userCourse.CourseId;
                    usercourseViewModel.CourseName = userCourse.CourseName;
                    usercourseViewModel.CourseContent = userCourse.CourseContent;
                    usercourseViewModel.Link = userCourse.Link;
                    usercourseViewModel.SubTopics = userCourse.SubTopics;
                    usercourseViewModel.Status = string.IsNullOrEmpty(userCourse.Status)? "Active": userCourse.Status;
                    usercourseViewModel.DueDate = userCourse.DueDate;
                    usercoursesViewModel.Add(usercourseViewModel);
                }

                return usercoursesViewModel;
            }
            catch (Exception ex)
            {
                throw new Exception("Error occured when get a user courses", ex);
            }
        }

        public async Task<string> AddUserCourse(IEnumerable<UserCourse> userCourses)
        {
            try
            {
                foreach(var userCourse in userCourses)
                {
                    userCourse.UserCourseId = Guid.NewGuid().ToString();
                    await this.dynamoDBUserCourseRepository.AddUserCourse(userCourse);
                }
                return string.Empty;
            }
            catch (Exception ex)
            {
                throw new Exception("Error occured when get a user", ex);
            }
        }      
        
    }
}
