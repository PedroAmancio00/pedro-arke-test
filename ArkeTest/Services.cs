using ArkeTest.Services;
using ArkeTest.Services.Login;
using ArkeTest.Services.Login.ILogin;
using ArkeTest.Services.User;
using ArkeTest.Services.User.IUser;

namespace ArkeTest
{
    public static class ArkeServices
    {
        public static void AddServices(this IServiceCollection services)
        {
            services.AddHttpContextAccessor();
            services.AddScoped<ICreateLoginService, CreateLoginService>();
            services.AddScoped<IAccessAccountService, AccessAccountService>();
            services.AddScoped<IRefreshService, RefreshService>();
            services.AddScoped<IJwtService, JwtService>();
            services.AddScoped<ILogoutService, LogoutService>();
            services.AddScoped<IChangePasswordService, ChangePasswordService>();
            services.AddScoped<ICreateUser, CreateOrUpdateUserService>();
        }
    }
}