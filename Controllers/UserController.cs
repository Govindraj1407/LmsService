using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Models;
using System;
using Elms.Services;

namespace Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UserController : ControllerBase
    {
        private readonly IUserService userService;

        /// <summary>
        /// Gets or sets user course service
        /// </summary>
        private readonly IUserCourseService userCourseService;

        public UserController(IUserService userService, IUserCourseService userCourseService)
        {
            this.userService = userService;
            this.userCourseService = userCourseService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var result = await this.userService.GetAllUser();

            if (result != null)
            {
                return this.Ok(result);
            }

            return this.NoContent();
        }

        [Route("{UserId}/")]
        [HttpGet]
        public async Task<IActionResult> Get(string UserId)
        {
            if (UserId != null)
            {
                var result = await this.userService.GetUser(UserId);

                if (result != null)
                {
                    return this.Ok(result);
                }

                return this.NoContent();
            }

            return this.BadRequest("Invalid request, please check the request parameter.");
        }

        [Route("{UserId}/Courses")]
        [HttpGet]
        public async Task<IActionResult> GetCourse(string UserId)
        {
            if (UserId != null)
            {
                var result = await this.userCourseService.GetUserCourses(UserId);

                if (result != null)
                {
                    return this.Ok(result);
                }

                return this.NoContent();
            }

            return this.BadRequest("Invalid request, please check the request parameter.");
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] User user)
        {
            if (this.ModelState.IsValid)
            {
                var message = await this.userService.CreateUser(user);
                if (string.IsNullOrEmpty(message))
                {
                    return this.Ok();
                }
                else
                {
                    return this.BadRequest(message);
                }

            }
            else
            {
                return this.BadRequest("Invalid request, please check the request parameter.");
            }

        }

        [HttpPut]
        public async Task<IActionResult> Update([FromBody] User user)
        {
            if (this.ModelState.IsValid)
            {
                var message = await this.userService.UpdateUser(user);
                if (string.IsNullOrEmpty(message))
                {
                    return this.Ok();
                }
                else
                {
                    return this.BadRequest(message);
                }
            }
            else
            {
                return this.BadRequest("Invalid request, please check the request parameter.");
            }

        }

        [Route("{UserId}/")]
        [HttpDelete]
        public async Task<IActionResult> Delete(string id)
        {
            if (this.ModelState.IsValid)
            {
                var message = await this.userService.DeleteUser(id);
                if (string.IsNullOrEmpty(message))
                {
                    return this.Ok();
                }
                else
                {
                    return this.BadRequest(message);
                }
            }
            else
            {
                return this.BadRequest("Invalid request, please check the request parameter.");
            }

        }

        [Route("Verify")]
        [HttpGet]
        public async Task<IActionResult> Verify(string username, string password)
        {
            if (this.ModelState.IsValid)
            {
                var user = await this.userService.VerifyLogin(username, password);
                if (user != null)
                {
                    return this.Ok(user);
                }
                else
                {
                    return this.BadRequest("Login Failed");
                }
            }
            else
            {
                return this.BadRequest("Invalid request, please check the request parameter.");
            }

        }
    }
}
