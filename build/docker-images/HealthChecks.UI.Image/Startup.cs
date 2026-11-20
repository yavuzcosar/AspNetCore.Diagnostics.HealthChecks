using HealthChecks.UI.Image.Configuration;
using HealthChecks.UI.Image.PushService;

namespace HealthChecks.UI.Image;

public class Startup
{
    public Startup(IConfiguration configuration)
    {
        Configuration = configuration;
    }

    public IConfiguration Configuration { get; }

    public void ConfigureServices(IServiceCollection services)
    {
        // SetCompatibilityVersion ASP.NET Core 11'de kaldırıldı (ASP5001 ile eskimişti); çağrının etkisi yoktu.
        services
            .AddHealthChecksUI()
            .AddStorageProvider(Configuration)
            .AddMvc();

        if (bool.TryParse(Configuration[PushServiceKeys.Enabled], out bool enabled) && enabled)
        {
            if (string.IsNullOrEmpty(Configuration[PushServiceKeys.PushEndpointSecret]))
            {
                throw new Exception($"{PushServiceKeys.PushEndpointSecret} environment variable has not been configured");
            }
            services.AddTransient<HealthChecksPushService>();
        }
    }
    public void Configure(IApplicationBuilder app)
    {
        app.UseRouting()
            .UseEndpoints(config =>
            {
                config.MapHealthChecksUI(Configuration);
                config.MapDefaultControllerRoute();
            });
    }
}
