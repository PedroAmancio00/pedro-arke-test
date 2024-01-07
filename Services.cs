using Microsoft.Extensions.DependencyInjection;
using ArkeTest.Services.Login;

namespace ArkeTest
{
    public static class ArkeServices
    {
        public static void AddServices(this IServiceCollection services)
        {
            services.AddScoped<CreateLoginService>();
        }
    }
}