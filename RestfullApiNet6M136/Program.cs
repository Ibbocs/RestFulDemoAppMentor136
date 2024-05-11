using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using RestfullApiNet6M136.Abstraction.IRepositories;
using RestfullApiNet6M136.Abstraction.IRepositories.ISchoolRepos;
using RestfullApiNet6M136.Abstraction.IRepositories.IStudentRepos;
using RestfullApiNet6M136.Abstraction.IUnitOfWorks;
using RestfullApiNet6M136.Abstraction.Services;
using RestfullApiNet6M136.Context;
using RestfullApiNet6M136.Entities.Identity;
using RestfullApiNet6M136.Extentions;
using RestfullApiNet6M136.Implementation.Repositories;
using RestfullApiNet6M136.Implementation.Repositories.EntitiesRepos;
using RestfullApiNet6M136.Implementation.Services;
using RestfullApiNet6M136.Implementation.UnitOfWorks;
using RestfullApiNet6M136.Mapping;
using RestfullApiNet6M136.Validations;
using Serilog;
using Serilog.Context;
using Serilog.Core;
using Serilog.Events;
using Serilog.Sinks.MSSqlServer;
using System.Collections.ObjectModel;
using System.Data;
using System.Security.Claims;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers().AddFluentValidation(config => config.RegisterValidatorsFromAssemblyContaining<SchoolCreateDTOValidator>());

builder.Services.UseCustomValidationResponse();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

//string connection = "Data Source=DESKTOP-PHRL2VS;Initial Catalog=MovieDB;Integrated Security=True";
//builder.Services.AddDbContext<AppDbContext>(options => options.UseSqlServer(connection));

builder.Services.AddDbContext<AppDbContext>(options => 
    options.UseSqlServer(builder.Configuration.GetConnectionString("Mentor136ApiDB")));

//identity
builder.Services.AddIdentity<AppUser, AppRole>().AddEntityFrameworkStores<AppDbContext>().AddDefaultTokenProviders();

//service registrartion
builder.Services.AddScoped<ISchoolService, SchoolService>();
builder.Services.AddScoped<IStudentService, StudentService>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IRoleService, RoleService>();
builder.Services.AddScoped<IAuthoService, AuthService>();
builder.Services.AddScoped<ITokenHandler, RestfullApiNet6M136.Implementation.Services.TokenHandler>();

//RepositoryRegistration
//builder.Services.AddScoped<IRepository, Repository>();
builder.Services.AddScoped(typeof(IRepository<>), typeof(Repository<>));

builder.Services.AddScoped<ISchoolRepository, SchoolRepo>();
builder.Services.AddScoped<IStudentRepository, StudentRepo>();

//builder.Services.AddScoped(typeof(IUnitOfWork<>), typeof(UnitOfWork<>));
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

//AutoMapper IoC elave elemeyin 2 yolu:
builder.Services.AddAutoMapper(typeof(MapProfile));
//builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

builder.Services.AddHttpContextAccessor(); //context gormek ucun http

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;

}).AddJwtBearer(options =>
{
    options.TokenValidationParameters = new()
    {
        ValidateAudience = true,//tokunumuzu kim/hansi origin islede biler
        ValidateIssuer = true, //tokunu kim palylayir
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true, //tokenin ozel keyi

        ValidAudience = builder.Configuration["Token:Audience"],

        ValidIssuer = builder.Configuration["Token:Issure"],

        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Token:SecurityKey"])),

        //token omru qeder islemesi ucun
        LifetimeValidator = (notBefore, expires, securityToken, validationParameters) => expires != null ? expires > DateTime.UtcNow : false,

        NameClaimType = ClaimTypes.Name,
        RoleClaimType = ClaimTypes.Role
    };
});

builder.Services.AddSwaggerGen(swagger =>
{
    //This is to generate the Default UI of Swagger Documentation  
    swagger.SwaggerDoc("v1", new OpenApiInfo
    {
        Version = "v1",
        Title = "Restaurant Final API",
        Description = "ASP.NET Core 6 Web API"
    });
    // To Enable authorization using Swagger (JWT)  
    swagger.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme()
    {
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "JWT Authorization header using the Bearer scheme."
    });
    swagger.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                          new OpenApiSecurityScheme
                            {
                                Reference = new OpenApiReference
                                {
                                    Type = ReferenceType.SecurityScheme,
                                    Id = "Bearer"
                                }
                            },
                            new string[] {}

                    }
                });
});

Logger? log = new LoggerConfiguration()
    .WriteTo.Console(LogEventLevel.Debug)
    .WriteTo.File("Logs/myJsonLogs.json")
    .WriteTo.File("Logs/mylogs.txt", outputTemplate: "{Timestamp:yyyy-MM-dd HH:mm:ss.fff zzz} [{Level:u3}] {Message:lj}{NewLine}{Exception}")
    .WriteTo.MSSqlServer(builder.Configuration.GetConnectionString("Mentor136ApiDB"), sinkOptions: new MSSqlServerSinkOptions
    {
        TableName = "MySeriLog",
        AutoCreateSqlTable = true
    },
    null, null, LogEventLevel.Warning, null,
    columnOptions: new ColumnOptions
    {
        AdditionalColumns = new Collection<SqlColumn>
        {
                new SqlColumn(columnName:"User_Name", SqlDbType.NVarChar)
        }
    },
     null, null
     )
    .Enrich.FromLogContext()
    .MinimumLevel.Information()
    .CreateLogger();

Log.Logger = log;

builder.Host.UseSerilog(log);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}


app.UseHttpsRedirection();

app.ConfigureExceptionHandler(/*app.Services.GetRequiredService<ILogger<Program>>()*/);

//serilogun requestleri loglamasi ucun
app.UseSerilogRequestLogging();


app.UseAuthentication();
app.UseAuthorization();

app.Use(async (context, next) =>
{
    //var userId = context.User?.Identity?.IsAuthenticated == true ? context.User.Identity.GetUserId() : null;
    //context.User.FindFirst(ClaimTypes.NameIdentifier);
    var username = context.User?.Identity?.IsAuthenticated != null || true ? context.User.Identity.Name : null;
    LogContext.PushProperty("User_Name", username);
    await next(context);
});


app.MapControllers();

app.Run();
