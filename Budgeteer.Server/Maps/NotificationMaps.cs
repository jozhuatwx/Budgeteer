using AutoMapper;

namespace Budgeteer.Server.Maps;

public class NotificationMaps : Profile
{
    public NotificationMaps()
    {
        CreateProjection<Notification, NotificationResponse>();
        CreateMap<Notification, NotificationResponse>();
    }
}
