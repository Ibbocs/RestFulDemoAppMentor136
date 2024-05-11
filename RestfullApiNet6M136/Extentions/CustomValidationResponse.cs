using Microsoft.AspNetCore.Mvc;
using RestfullApiNet6M136.Models;

namespace RestfullApiNet6M136.Extentions
{
    public static class CustomValidationResponse
    {
        public static void UseCustomValidationResponse(this IServiceCollection service)
        {
            service.Configure<ApiBehaviorOptions>(options =>
            {
                options.InvalidModelStateResponseFactory = context =>
                {
                    var errors = context.ModelState.Values.Where(x => x.Errors.Count > 0)
                    .SelectMany(x => x.Errors).Select(x => x.ErrorMessage);

                    ErrorDTO errorDto = new ErrorDTO(errors.ToList());

                    var response = new GenericResponseModel<ErrorDTO> { Data = errorDto, StatusCode = 400 };
                    return new BadRequestObjectResult(response);
                };
            });
        }
    }
}
