using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using RestfullApiNet6M136.Abstraction.Services;
using RestfullApiNet6M136.Context;
using RestfullApiNet6M136.DTOs.SchoolDTOs;
using RestfullApiNet6M136.Models;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace RestfullApiNet6M136.Implementation.Services
{
    public class SchoolService : ISchoolService
    {
        private readonly IMapper mapper;
        private readonly AppDbContext appDbContext;


        public SchoolService(IMapper _mapper, AppDbContext appDbContext)
        {
            mapper = _mapper;
            this.appDbContext = appDbContext;
        }

        public async Task<GenericResponseModel<SchoolCreateDTO>> AddSchool(SchoolCreateDTO model)
        {
            try
            {
                if (model != null)
                {
                    await appDbContext.Schools.AddAsync(new()
                    {
                        Number = model.Number,
                        Name = model.Name
                    });



                    int result = await appDbContext.SaveChangesAsync();

                    if (result > 0)
                    {
                        return new GenericResponseModel<SchoolCreateDTO>
                        {
                            Data = model,
                            StatusCode = 201,
                        };
                    }
                    else
                    {
                        return new GenericResponseModel<SchoolCreateDTO>
                        {
                            Data = model,
                            StatusCode = 500,
                        };

                        //Sadece xeta cixar-refactoring
                    }
                }
                else
                {
                    return new GenericResponseModel<SchoolCreateDTO>
                    {
                        Data = model,
                        StatusCode = 400,
                    };
                }
            }
            catch (Exception ex)
            {
                //await Console.Out.WriteLineAsync(ex.Message);
                Console.WriteLine(ex.Message + "Inner Ex:" + ex.InnerException);

                return new GenericResponseModel<SchoolCreateDTO>
                {
                    Data = model,
                    StatusCode = 500,
                };
            }
            finally
            {
                //appDbContext.Dispose();
            }
           
        }

        public async Task<GenericResponseModel<bool>> DeleteSchool(int Id)
        {
            try
            {
                var data= await appDbContext.Schools.FirstOrDefaultAsync(id=> id.Id == Id);


                if (data != null)
                {
                    appDbContext.Schools.Remove(data);
                    var rowAffect = await appDbContext.SaveChangesAsync();

                    if (rowAffect>0)
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
                            StatusCode = 400,
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
            catch (Exception ex)
            {

                Console.WriteLine(ex.Message + "Inner Ex:" + ex.InnerException);
                return new GenericResponseModel<bool>
                {
                    Data = false,
                    StatusCode = 500,
                };
            }
        }

        public async Task<GenericResponseModel<List<SchoolGetDTO>>> GetAllSchool()
        {
            try
            {
                var data = await appDbContext.Schools.ToListAsync();

                if (data != null && data.Count > 0)
                {
                    var dtos = mapper.Map<List<SchoolGetDTO>>(data);

                    return new GenericResponseModel<List<SchoolGetDTO>>
                    {
                        Data = dtos,
                        StatusCode = 200,
                    };
                }
                else
                {
                    return new GenericResponseModel<List<SchoolGetDTO>>
                    {
                        Data = null,
                        StatusCode = 404,
                    };
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message + "Inner Ex:" + ex.InnerException);
                return new GenericResponseModel<List<SchoolGetDTO>>
                {
                    Data = null,
                    StatusCode = 500,
                };
            }

            
        }

        public async Task<GenericResponseModel<SchoolGetDTO>> GetSchoolById(int Id)
        {
            var data = await appDbContext.Schools.FirstOrDefaultAsync(data => data.Id == Id);

            if (data != null)
            {
                var dtos = mapper.Map<SchoolGetDTO>(data);

                return new GenericResponseModel<SchoolGetDTO>
                {
                    Data = dtos,
                    StatusCode = 200
                };
            }
            else 
            {
                return new GenericResponseModel<SchoolGetDTO>
                {
                    Data = null,
                    StatusCode = 404
                };
            }
        }

        public async Task<GenericResponseModel<bool>> UpdateSchool(SchoolUpdateDTO model)
        {
            try
            {
                var data = await appDbContext.Schools.FirstOrDefaultAsync(id => id.Id == model.Id);


                if (data != null)
                {
                    data.Number = model.Number;
                    data.Name = model.Name;

                    appDbContext.Schools.Update(data);

                    var rowAffect = await appDbContext.SaveChangesAsync();

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
            catch (Exception ex)
            {

                Console.WriteLine(ex.Message + "Inner Ex:" + ex.InnerException);
                return new GenericResponseModel<bool>
                {
                    Data = false,
                    StatusCode = 500,
                };
            }
        }
    }
}
