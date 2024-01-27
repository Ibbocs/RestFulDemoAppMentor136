using AutoMapper;
using RestfullApiNet6M136.DTOs.SchoolDTOs;
using RestfullApiNet6M136.DTOs.StudentDTOs;
using RestfullApiNet6M136.DTOs.UserDTOs;
using RestfullApiNet6M136.Entities.AppdbContextEntity;
using RestfullApiNet6M136.Entities.Identity;

namespace RestfullApiNet6M136.Mapping;

public class MapProfile:Profile
{
    public MapProfile()
    {
        //School entitisinden dtosuna cevir
        CreateMap<School,SchoolGetDTO>().ReverseMap(); 

        //dtoda olan school name hardan alacagin deyirem:School icindeki nameden
        //qeyd db School entitisi de gelmis olmalidir ha. Ya include edesen ya da get sorgusu ile ram alasan
        CreateMap<Student,StudentGetDTO>()
            .ForMember(dest=> dest.SchoolName, option=> option.MapFrom(src=> src.School.Name))
            .ReverseMap();

        CreateMap<AppUser, UserGetDTO>().ReverseMap();

        
    }
}
