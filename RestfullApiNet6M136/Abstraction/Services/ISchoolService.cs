using RestfullApiNet6M136.DTOs.SchoolDTOs;
using RestfullApiNet6M136.Models;

namespace RestfullApiNet6M136.Abstraction.Services
{
    public interface ISchoolService
    {
        Task<GenericResponseModel<SchoolCreateDTO>> AddSchool(SchoolCreateDTO model);
        Task<GenericResponseModel<bool>> DeleteSchool(int Id);
        Task<GenericResponseModel<bool>> UpdateSchool(SchoolUpdateDTO model);

        Task<GenericResponseModel<List<SchoolGetDTO>>> GetAllSchool();
        Task<GenericResponseModel<SchoolGetDTO>> GetSchoolById(int Id);
    }
}
