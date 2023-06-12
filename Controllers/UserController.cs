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

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] User user)
        {
            if(this.ModelState.IsValid)
            {
                await this.userService.CreateUser(user);
                return this.NoContent();
            }
            else
            {
                return this.BadRequest("Invalid request, please check the request parameter.");
            }
           
        }
    }
}
