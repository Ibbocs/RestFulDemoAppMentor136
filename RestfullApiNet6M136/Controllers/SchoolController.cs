using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RestfullApiNet6M136.Abstraction.Services;
using RestfullApiNet6M136.DTOs.SchoolDTOs;

namespace RestfullApiNet6M136.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SchoolController : ControllerBase
    {
        private readonly ISchoolService _schoolService;
        public SchoolController(ISchoolService schoolService)
        {
            _schoolService = schoolService;
        }

        [HttpGet("[action]")]
        [Authorize(Roles = "Admin,User")]
        public async Task<IActionResult> GetAllSchool()
        {
            var data = await _schoolService.GetAllSchool();
            return StatusCode(data.StatusCode, data);
        }

        [HttpGet("[action]/{Id}")]//FromQuery falan yazsam lazim deyil burda id almaq,amma burda  alanda is required olur
        [Authorize(Roles = "Admin,User")]
        public async Task<IActionResult> GetSchool(int Id)
        {
            var data = await _schoolService.GetSchoolById(Id);
            return StatusCode(data.StatusCode, data);
        }

        [HttpPost("[action]")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> CreateSchool([FromBody] SchoolCreateDTO model)
        {
            var data = await _schoolService.AddSchool(model);
            return StatusCode(data.StatusCode, data);
        }

        [HttpPut("[action]")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> UpdateSchool([FromBody] SchoolUpdateDTO model)
        {
            var data = await _schoolService.UpdateSchool(model);
            return StatusCode(data.StatusCode, data);
        }

        [HttpDelete("[action]")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteSchool([FromQuery] int Id)
        {
            var data = await _schoolService.DeleteSchool(Id);
            return StatusCode(data.StatusCode, data);
        }
    }
}
