using ArkeTest;
using ArkeTest.Data;
using ArkeTest.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<MyDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddIdentityApiEndpoints<ApplicationUser>(options =>
    {
        options.User.RequireUniqueEmail = true;
    })
    .AddEntityFrameworkStores<MyDbContext>();

builder.Services.AddControllers();

builder.Services.AddServices();

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = false,
        ValidateAudience = false,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes("bf5ec0cf8bdd34c7508f8d40a7df96b32ff4f2699b96f88076dc9b746b01eb82"))
    };
    options.Events = new JwtBearerEvents
    {
        OnMessageReceived = context =>
        {
            if (context.Request.Cookies.ContainsKey("jwt"))
            {
                context.Token = context.Request.Cookies["jwt"];
            }
            return Task.CompletedTask;
        }
    };
});

builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "ArkeTest API", Version = "v1" });
    c.EnableAnnotations();
});

builder.Logging.AddConsole();

var app = builder.Build();

ApplyMigrations(app);

Configure(app, app.Environment);

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.MapControllers();

app.Run();

static void ApplyMigrations(WebApplication app)
{
    using var scope = app.Services.CreateScope();
    var db = scope.ServiceProvider.GetRequiredService<MyDbContext>();
    db.Database.Migrate();
}

static void Configure(IApplicationBuilder app, IWebHostEnvironment env)
{

    app.Use(async (context, next) =>
    {
        var token = context.Request.Cookies["jwt"];
        if (!string.IsNullOrEmpty(token))
        {
            var jwtSecurityToken = new JwtSecurityTokenHandler().ReadJwtToken(token);
            var expiry = jwtSecurityToken.ValidTo;
            if(expiry > DateTime.Now)
            {
                context.Request.Headers.Append("Authorization", "Bearer " + token);
            }  
            else
            {
                context.Response.Cookies.Delete("jwt");
            }
        }
        await next.Invoke();
    });

    app.UseAuthentication();
    app.UseAuthorization();

}