using AutoMapper;

namespace Playground.Server.Maps;

public class NotificationMaps : Profile
{
    public NotificationMaps()
    {
        CreateProjection<Notification, NotificationResponse>();
        CreateMap<Notification, NotificationResponse>();
    }
}
