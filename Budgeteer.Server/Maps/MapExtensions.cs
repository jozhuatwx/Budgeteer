namespace Budgeteer.Server.Maps;

public static class MapExtensions
{
	public static IServiceCollection AddMappings(this IServiceCollection services)
	{
		services
			.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

		return services;
	}
}
