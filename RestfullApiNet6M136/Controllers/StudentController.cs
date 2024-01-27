using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RestfullApiNet6M136.Abstraction.Services;
using RestfullApiNet6M136.DTOs.StudentDTOs;

namespace RestfullApiNet6M136.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StudentController : ControllerBase
    {
        private readonly IStudentService _studentService;
        public StudentController(IStudentService studentService)
        {
            _studentService = studentService;
        }

        [HttpGet("[action]")]
        //[Authorize(AuthenticationSchemes = "Admin", Roles = "Admin")]
        public async Task<IActionResult> GetAllStudent()
        {
            var data = await _studentService.GetAllStudent();
            return StatusCode(data.StatusCode, data);
        }

        [HttpGet("[action]/{Id}")]
        //[Authorize(AuthenticationSchemes = "Admin", Roles = "Admin")]
        public async Task<IActionResult> GetStudent(int Id)
        {
            var data = await _studentService.GetStudentById(Id);
            return StatusCode(data.StatusCode, data);
        }

        //[HttpGet("[action]/{Id}")]
        ////[Authorize(AuthenticationSchemes = "Admin", Roles = "Admin,User")]
        //public async Task<IActionResult> GetStudentByUserId(int Id)
        //{
        //    var data = await _studentService.GetAllStudentByUserId(Id);
        //    return StatusCode(data.StatusCode, data);
        //}

        [HttpPost("[action]")]
        //[Authorize(AuthenticationSchemes = "Admin", Roles = "Admin,User")]
        public async Task<IActionResult> CreateStudent([FromBody] StudentCreateDTO model)
        {
            var data = await _studentService.AddStudent(model);
            return StatusCode(data.StatusCode, data);

        }

        [HttpPut("[action]")]
        //[Authorize(AuthenticationSchemes = "Admin", Roles = "Admin")]
        public async Task<IActionResult> ChangeStudent([FromBody]ChangeSchoolDTO model)
        {
            var data = await _studentService.ChangeSchool(model);
            return StatusCode(data.StatusCode, data);

        }

        [HttpPut("[action]/{studentId}/{newStudentId}")]
        //[Authorize(AuthenticationSchemes = "Admin", Roles = "Admin")]
        public async Task<IActionResult> CahngeStudent(int studentId, int newStudentId)
        {
            var data = await _studentService.ChangeSchool(studentId, newStudentId);
            return StatusCode(data.StatusCode, data);

        }

        [HttpPut("[action]")]
        //[Authorize(AuthenticationSchemes = "Admin", Roles = "User")]
        public async Task<IActionResult> UpdateStudent([FromBody]StudentUpdateDTO model, [FromQuery]int id)
        {
            var data = await _studentService.UpdateStudent(model, id);
            return StatusCode(data.StatusCode, data);
        }

        [HttpDelete("[action]")]
        //[Authorize(AuthenticationSchemes = "Admin", Roles = "User")]
        public async Task<IActionResult> DeleteStudent([FromQuery] int Id)
        {
            var data = await _studentService.DeleteStudent(Id);
            return StatusCode(data.StatusCode, data);
        }
    }
}
