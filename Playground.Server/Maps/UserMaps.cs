using AutoMapper;

namespace Playground.Server.Maps;

public class UserMaps : Profile
{
    public UserMaps()
    {
        CreateProjection<User, UserResponse>();
        CreateMap<User, UserResponse>();
    }
}
