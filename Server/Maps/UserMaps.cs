using AutoMapper;

namespace Playground.Server.Maps;

public class UserMaps : Profile
{
    public UserMaps()
    {
        CreateMap<User, UserResponse>();
    }
}
