namespace ControleFinanceiro.Infrastructure.Extensions;

public static class ServiceCollectionExternsions
{
    public static IServiceCollection ConfigureCors(this IServiceCollection services, IConfiguration configuration)
    {
        var frontendUrl = configuration["FRONTEND_URL"];
        if (string.IsNullOrEmpty(frontendUrl))
        {
            throw new Exception("FRONTEND_URL environment variable is not set.");
        }

        services.AddCors(options =>
        {
            options.AddPolicy("AllowFrontend",
                policy =>
                {
                    policy
                        .WithOrigins(frontendUrl)
                        .AllowAnyHeader()
                        .AllowAnyMethod();
                });
        });

        return services;
    }
}
