using Microsoft.AspNetCore.Mvc;
using Services;
using System.Threading.Tasks;
using ViewModels;

namespace Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UserController : ControllerBase
    {
        private readonly IUserService userService;

        public UserController(IUserService userService)
        {
            this.userService = userService;
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

        [Route("{UserId:int}/")]
        [HttpGet]
        public async Task<IActionResult> Get(int UserId)
        {
            if (UserId > 0)
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

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] User user)
        {
            if (this.ModelState.IsValid)
            {
                var message = await this.userService.CreateUser(user);
                if (string.IsNullOrEmpty(message))
                {
                    message = "User created successfully";
                    return this.Ok(message);
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
                    message = "User updated successfully";
                    return this.Ok(message);
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

        [Route("{UserId:int}/")]
        [HttpDelete]
        public async Task<IActionResult> Delete(int id)
        {
            if (this.ModelState.IsValid)
            {
                var message = await this.userService.DeleteUser(id);
                if (string.IsNullOrEmpty(message))
                {
                    message = "User deleted successfully";
                    return this.Ok(message);
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
    }
}
