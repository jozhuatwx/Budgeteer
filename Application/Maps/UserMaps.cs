using AutoMapper;

namespace Playground.Application.Maps;

public class UserMaps : Profile
{
    public UserMaps()
    {
        CreateMap<User, UserResponse>();
    }
}
