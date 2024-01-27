using RestfullApiNet6M136.DTOs.StudentDTOs;
using RestfullApiNet6M136.Entities.AppdbContextEntity;
using RestfullApiNet6M136.Models;

namespace RestfullApiNet6M136.Abstraction.Services
{
    public interface IStudentService
    {
        Task<GenericResponseModel<StudentCreateDTO>> AddStudent(StudentCreateDTO model);
        Task<GenericResponseModel<bool>> DeleteStudent(int Id);
        Task<GenericResponseModel<bool>> UpdateStudent(StudentUpdateDTO model, int Id);

        Task<GenericResponseModel<List<StudentGetDTO>>> GetAllStudent();
        Task<GenericResponseModel<StudentGetDTO>> GetStudentById(int Id);
        Task<GenericResponseModel<List<StudentGetDTO>>> GetAllStudentBySchoolId(int Id);

        //ChangeScholl-refactoring
        Task<GenericResponseModel<bool>> ChangeSchool(int studentId, int newSchoolId);
        Task<GenericResponseModel<bool>> ChangeSchool(ChangeSchoolDTO model);

        //public  Task<Student> GetStudentId(int Id);
    }
}
