using AutoMapper;
using Azure.Core;
using Microsoft.AspNetCore.Mvc;
using RestfullApiNet6M136.Abstraction.IRepositories.IStudentRepos;
using RestfullApiNet6M136.Abstraction.IUnitOfWorks;
using RestfullApiNet6M136.Abstraction.Services;
using RestfullApiNet6M136.DTOs.StudentDTOs;
using RestfullApiNet6M136.Entities.AppdbContextEntity;
using Serilog;
using Serilog.Context;

namespace RestfullApiNet6M136.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        private static readonly string[] Summaries = new[]
        {
        "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
    };

        private readonly ILogger<WeatherForecastController> _logger;
        private readonly IStudentService studentService;
        private readonly IMapper _mapper;
        private readonly IStudentRepository _repo;

        public WeatherForecastController(ILogger<WeatherForecastController> logger, IStudentService _studentService, IMapper mapper, IStudentRepository repo)
        {
            _logger = logger;
            studentService = _studentService;
            _mapper = mapper;
            _repo = repo;
        }

        [HttpGet]
        public void Logum()
        {
            ArgumentNullException argumentNull = new("MyParamName", "MyErrorMessage");

            Log.Error(argumentNull, "ArgumentNullException olu?tu: {ParamName}, {ErrorMessage}", argumentNull.ParamName, argumentNull.Message);
        }


        //[HttpGet(Name = "GetWeatherForecast")]
        //public IEnumerable<WeatherForecast> Get()
        //{
        //    return Enumerable.Range(1, 5).Select(index => new WeatherForecast
        //    {
        //        Date = DateTime.Now.AddDays(index),
        //        TemperatureC = Random.Shared.Next(-20, 55),
        //        Summary = Summaries[Random.Shared.Next(Summaries.Length)]
        //    })
        //    .ToArray();
        //}

        //[HttpGet()]
        //public async Task<IActionResult> MyGet(/*[FromBody]string query*/)
        //{
        //    var log = new LoggerConfiguration();

        //    Log.Information("salammmmm");
        //    Log.Error("salammmmmError");
        //    // using (LogContext.PushProperty("UserName", "Veli")) 
        //    LogContext.PushProperty("UserName", "Veli");

        //    //Log.CloseAndFlush();

        //    _logger.LogError("bakii Loggerden");

        //    throw new NotImplementedException();
        //    //return Ok(data);
        //}

        //[HttpGet("[action]")]
        //public async Task<StudentListModel> Index([FromQuery] PageRequest request)
        //{
        //    IPaginate<Student> brands = await _repo.GetListAsync(index: request.Page, size: request.PageSize);

        //    StudentListModel mappedBrandListModel = _mapper.Map<StudentListModel>(brands);

        //    return mappedBrandListModel;
        //}
    }
}