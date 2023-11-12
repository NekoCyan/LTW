namespace LTW
{
    public class Startup : IStartup
    {
        public IServiceProvider ConfigureServices(IServiceCollection services)
        {
            // Register services
            services.AddHttpClient();

            return (IServiceProvider)services;
        }

        public void Configure(IApplicationBuilder app)
        {
        }
    }
}
