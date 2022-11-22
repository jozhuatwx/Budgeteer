using AutoMapper;

namespace Budgeteer.Server.Maps;

public class UserMaps : Profile
{
    public UserMaps()
    {
        CreateProjection<User, UserResponse>();
        CreateMap<User, UserResponse>();
    }
}
