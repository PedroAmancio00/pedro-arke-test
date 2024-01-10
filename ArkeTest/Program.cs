using ArkeTest;
using ArkeTest.Data;
using ArkeTest.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

var configuration = builder.Configuration;

builder.Services.AddDbContext<MyDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddIdentityApiEndpoints<ApplicationUser>(options =>
    {
        options.User.RequireUniqueEmail = true;
    })
    .AddEntityFrameworkStores<MyDbContext>();

builder.Services.AddControllers();

builder.Services.AddServices();

var key = configuration["jwtKey"]!;

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
            Encoding.UTF8.GetBytes(key))
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

WebApplication app = builder.Build();

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
    using IServiceScope scope = app.Services.CreateScope();
    MyDbContext db = scope.ServiceProvider.GetRequiredService<MyDbContext>();
    db.Database.Migrate();
}

static void Configure(IApplicationBuilder app, IWebHostEnvironment env)
{

    app.Use(async (context, next) =>
    {
        string? token = context.Request.Cookies["jwt"];
        if (!string.IsNullOrEmpty(token))
        {
            context.Request.Headers.Append("Authorization", "Bearer " + token);
        }
        await next.Invoke();
    });

    app.UseAuthentication();
    app.UseAuthorization();

}