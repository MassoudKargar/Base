using AutoMapper;

namespace Base.Extensions.ObjectMappers.AutoMapper.Sample.Models;

public class PersonStudentAutoMapperProfile : Profile
{
    public PersonStudentAutoMapperProfile()
    {
        CreateMap<Person, Student>().ReverseMap();
    }
}
