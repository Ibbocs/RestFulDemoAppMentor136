using AutoMapper;
using Microsoft.EntityFrameworkCore;
using RestfullApiNet6M136.Abstraction.IRepositories;
using RestfullApiNet6M136.Abstraction.IUnitOfWorks;
using RestfullApiNet6M136.Abstraction.Services;
using RestfullApiNet6M136.DTOs.StudentDTOs;
using RestfullApiNet6M136.Entities.AppdbContextEntity;
using RestfullApiNet6M136.Models;

namespace RestfullApiNet6M136.Implementation.Services
{
    public class StudentService : IStudentService
    {
        private readonly IMapper mapper;
        private readonly IUnitOfWork _unitOfWork;
        private IRepository<Student> _student;
        private IRepository<School> _school;


        public StudentService(IMapper _mapper, IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;//bu yuxarida olmalidi, yoxsa null olur deye Repolar gelmir!!!

            this._student = _unitOfWork.GetRepository<Student>();
            this._school = _unitOfWork.GetRepository<School>();

            mapper = _mapper;

        }

        public async Task<GenericResponseModel<StudentCreateDTO>> AddStudent(StudentCreateDTO model)
        {
            #region MyRegion
            //var studentEntity = _studentRepository.Insert(new Student
            //{
            //    // Model verilerini entity verilerine dönüştürme
            //    Name = model.Name,
            //    Age = model.Age
            //    // Diğer özellikler
            //});

            //_unitOfWork.SaveChangesAsync();

            //await _unitOfWork.GetRepository<Student>().GetByIdAsync(2);
            // await _unitOfWork.SaveChangesAsync();
            #endregion


            if (model != null)
            {
                /*bool addResult =*/
                await _student.AddAsync(new()
                {
                    Name = model.Name,
                    SchoolId = model.SchoolId,

                });

                int result = await _unitOfWork.SaveChangesAsync();

                if (result > 0)
                {
                    return new GenericResponseModel<StudentCreateDTO>
                    {
                        Data = model,
                        StatusCode = 201,
                    };
                }
                else
                {
                    return new GenericResponseModel<StudentCreateDTO>
                    {
                        Data = model,
                        StatusCode = 500,
                    };

                    //Sadece xeta cixar-refactoring
                }

            }
            else
            {
                return new GenericResponseModel<StudentCreateDTO>
                {
                    Data = model,
                    StatusCode = 400,
                };
            }
        }

        public async Task<GenericResponseModel<bool>> ChangeSchool(int studentId, int newSchoolId)
        {
            var myStudent = await _student.GetByIdAsync(studentId);

            if (myStudent == null)
            {
                return new GenericResponseModel<bool>
                {
                    Data = false,
                    StatusCode = 400,
                };
            }

            var mySchool = await _school.GetByIdAsync(newSchoolId);//Any ile de yazmaq olardi amma repoda yoxdu

            if (mySchool == null)
            {
                return new GenericResponseModel<bool>
                {
                    Data = false,
                    StatusCode = 400,
                };
            }

            myStudent.SchoolId = newSchoolId;

            _student.Update(myStudent);


            var rowAffect = await _unitOfWork.SaveChangesAsync();

            if (rowAffect > 0)
            {
                return new GenericResponseModel<bool>
                {
                    Data = true,
                    StatusCode = 200,
                };
            }
            else
            {
                return new GenericResponseModel<bool>
                {
                    Data = false,
                    StatusCode = 500,
                };
            }
        }

        public async Task<GenericResponseModel<bool>> ChangeSchool(ChangeSchoolDTO model)
        {
            var myStudent = await _student.GetByIdAsync(model.StudentID);

            if (myStudent == null)
            {
                return new GenericResponseModel<bool>
                {
                    Data = false,
                    StatusCode = 400,
                };
            }

            var mySchool = await _school.GetByIdAsync(model.NewSchoolID);//Any ile de yazmaq olardi amma repoda yoxdu

            if (mySchool == null)
            {
                return new GenericResponseModel<bool>
                {
                    Data = false,
                    StatusCode = 400,
                };
            }

            myStudent.SchoolId = model.NewSchoolID;

            _student.Update(myStudent);


            var rowAffect = await _unitOfWork.SaveChangesAsync();

            if (rowAffect > 0)
            {
                return new GenericResponseModel<bool>
                {
                    Data = true,
                    StatusCode = 200,
                };
            }
            else
            {
                return new GenericResponseModel<bool>
                {
                    Data = false,
                    StatusCode = 500,
                };
            }
        }

        public async Task<GenericResponseModel<bool>> DeleteStudent(int Id)
        {

            var data = await _student.GetByIdAsync(Id);
             

            if (data != null)
            {
                _student.Remove(data);
                var rowAffect = await _unitOfWork.SaveChangesAsync();

                if (rowAffect > 0)
                {
                    return new GenericResponseModel<bool>
                    {
                        Data = true,
                        StatusCode = 200,
                    };
                }
                else
                {
                    return new GenericResponseModel<bool>
                    {
                        Data = false,
                        StatusCode = 500,
                    };
                }

            }
            else
            {
                return new GenericResponseModel<bool>
                {
                    Data = false,
                    StatusCode = 400,
                };
            }

        }

        public async Task<GenericResponseModel<List<StudentGetDTO>>> GetAllStudent()
        {

            var data = await _student.GetAll().Include(x => x.School).ToListAsync();//eager loading

            if (data != null && data.Count > 0)
            {
                var dtos = mapper.Map<List<StudentGetDTO>>(data);

                return new GenericResponseModel<List<StudentGetDTO>>
                {
                    Data = dtos,
                    StatusCode = 200,
                };
            }
            else
            {
                return new GenericResponseModel<List<StudentGetDTO>>
                {
                    Data = null,
                    StatusCode = 404,
                };
            }
        }

        public async Task<GenericResponseModel<List<StudentGetDTO>>> GetAllStudentBySchoolId(int Id)
        {
            throw new NotImplementedException();
        }

        public async Task<GenericResponseModel<StudentGetDTO>> GetStudentById(int Id)
        {
            var data = await _student.GetByIdAsync(Id);

            if (data != null)
            {
                await _school.GetByIdAsync(data.SchoolId);
                var dtos = mapper.Map<StudentGetDTO>(data);

                return new GenericResponseModel<StudentGetDTO>
                {
                    Data = dtos,
                    StatusCode = 200
                };
            }
            else
            {
                return new GenericResponseModel<StudentGetDTO>
                {
                    Data = null,
                    StatusCode = 404
                };
            }
        }

        public async Task<GenericResponseModel<bool>> UpdateStudent(StudentUpdateDTO model, int Id)
        {

            var data = await _student.GetByIdAsync(Id);


            if (data != null)
            {
                data.Name = model.Name;
                data.SchoolId = data.SchoolId;

                _student.Update(data);

                var rowAffect = await _unitOfWork.SaveChangesAsync();

                if (rowAffect > 0)
                {
                    return new GenericResponseModel<bool>
                    {
                        Data = true,
                        StatusCode = 200,
                    };
                }
                else
                {
                    return new GenericResponseModel<bool>
                    {
                        Data = false,
                        StatusCode = 500,
                    };
                }

            }
            else
            {
                return new GenericResponseModel<bool>
                {
                    Data = false,
                    StatusCode = 400,
                };
            }

        }

        //public async Task<Student> GetStudentId(int Id)
        //{

        //    var data1 = await _student.GetByIdAsync(Id);
        //    //var data = await _unitOfWork.StudentRepo.GetByIdAsync(Id);

        //    var data = await _unitOfWork.GetRepository<Student>().GetByIdAsync(Id);
        //    await _unitOfWork.SaveChangesAsync();
        //    return data;
        //}
    }
}
