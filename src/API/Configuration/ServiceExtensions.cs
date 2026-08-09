namespace API.Configuration;

public static class ServiceExtensions
{
    public const string CorsPolicyName = "Frontend";

    public static IServiceCollection AddConfiguration(this IServiceCollection services, IConfiguration configuration)
        => services
            .AddCustomCors(configuration);

    public static IServiceCollection AddCustomCors(this IServiceCollection services, IConfiguration configuration)
    {
        var allowedOrigins = configuration.GetSection("Cors:AllowedOrigins").Get<string[]>() ?? [];

        services.AddCors(options =>
        {
            options.AddPolicy(CorsPolicyName, policy =>
            {
                policy.SetIsOriginAllowed(origin =>
                        allowedOrigins.Contains(origin)
                        || (Uri.TryCreate(origin, UriKind.Absolute, out var originUri)
                            && originUri.Host is "localhost" or "127.0.0.1"))
                    .AllowAnyHeader()
                    .AllowAnyMethod();
            });
        });

        return services;
    }

}
