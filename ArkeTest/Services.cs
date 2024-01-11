using ArkeTest.Services.Factory;
using ArkeTest.Services.Factory.IFactory;
using ArkeTest.Services.FactoryWrapper;
using ArkeTest.Services.Jwt;
using ArkeTest.Services.Jwt.IJwt;
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
            services.AddScoped<ITokenValidationParametersFactory, TokenValidationParametersFactory>();
            services.AddScoped<ITokenHandlerWrapper, TokenHandlerWrapper>();
        }
    }
}