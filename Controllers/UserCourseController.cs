using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Models;
using System;
using System.Collections.Generic;
using Elms.Services;

namespace Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UserCourseController : ControllerBase
    {
        /// <summary>
        /// Gets or sets user course service
        /// </summary>
        private readonly IUserCourseService userCourseService;

        public UserCourseController(IUserCourseService userCourseService)
        {
            this.userCourseService = userCourseService;
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] IEnumerable<UserCourse> userCourses)
        {
                var message = await this.userCourseService.AddUserCourse(userCourses);
                if (string.IsNullOrEmpty(message))
                {
                   return this.Ok();
                }
                else
                {
                    return this.BadRequest(message);
                }

        }
    }
}
