using Microsoft.AspNetCore.Mvc;
using Services;
using System.Threading.Tasks;
using LMS.ViewModels;

namespace Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CourseWareController : ControllerBase
    {
        private readonly ICourseWareService courseService;

        public CourseWareController(ICourseWareService courseService)
        {
            this.courseService = courseService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var result = await this.courseService.GetAllCourse();

            if (result != null)
            {
                return this.Ok(result);
            }

            return this.NoContent();
        }

        [Route("{CourseId:int}/")]
        [HttpGet]
        public async Task<IActionResult> Get(int CourseId)
        {
            if (CourseId > 0)
            {
                var result = await this.courseService.GetCourse(CourseId);

                if (result != null)
                {
                    return this.Ok(result);
                }
                
                return this.NoContent();
            }

            return this.BadRequest("Invalid request, please check the request parameter.");
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] Course course)
        {
            if(this.ModelState.IsValid)
            {
                var message = await this.courseService.CreateCourse(course);
                if (string.IsNullOrEmpty(message))
                {
                    message = "Course created successfully";
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
        public async Task<IActionResult> Update([FromBody] Course course)
        {
            if (this.ModelState.IsValid)
            {
                var message = await this.courseService.UpdateCourse(course);
                if (string.IsNullOrEmpty(message))
                {
                    message = "Course updated successfully";
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

        [Route("{CourseId:int}/")]
        [HttpDelete]
        public async Task<IActionResult> Delete(int id)
        {
            if (this.ModelState.IsValid)
            {
                var message = await this.courseService.DeleteCourse(id);
                if (string.IsNullOrEmpty(message))
                {
                    message = "Course deleted successfully";
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
