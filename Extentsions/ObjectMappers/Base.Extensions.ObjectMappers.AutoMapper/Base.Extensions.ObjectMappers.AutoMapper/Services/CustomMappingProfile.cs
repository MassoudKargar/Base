namespace Base.Extensions.ObjectMappers.AutoMapper.Services;

public class CustomMappingProfile : Profile
{
    public CustomMappingProfile()
    {
    }
    public CustomMappingProfile(IEnumerable<IHaveCustomMapping?> haveCustomMappings) : this()
    {
        foreach (var item in haveCustomMappings)
        {
            if (item is not null)
            {
                item.CreateMappings(this);
            }
        }
    }
}