using Elms.Services;
using Microsoft.AspNetCore.Mvc;
using Models;
using System;
using System.Threading.Tasks;

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

        [Route("{CourseId}/")]
        [HttpGet]
        public async Task<IActionResult> Get(string CourseId)
        {
            if (CourseId != null)
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
            course.CourseId = Guid.NewGuid().ToString();
            if (this.ModelState.IsValid)
            {
                var message = await this.courseService.CreateCourse(course);
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
        public async Task<IActionResult> Update([FromBody] Course course)
        {
            if (this.ModelState.IsValid)
            {
                var message = await this.courseService.UpdateCourse(course);
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

        [Route("{CourseId}/")]
        [HttpDelete]
        public async Task<IActionResult> Delete(string CourseId)
        {
            if (this.ModelState.IsValid)
            {
                var message = await this.courseService.DeleteCourse(CourseId);
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
    }
}
