using Microsoft.EntityFrameworkCore;
using AuthService.Database;
using AuthService.Core.BusinessLogic;
using AuthService.Database.Mappers;
using AuthService.Core.HelperModels;
using AuthService.Core.Helpers;
using AuthService.Database.HelperModels;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using AuthService.Core.Mappers;
using AuthService.Database.Models;
using Microsoft.AspNetCore.Identity;
using AuthService.Database.Implements;
using AuthService.Database.Interfaces;
using AuthService.Main.Services;
using AuthService.Main.SettingModels;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
if (builder.Environment.IsDevelopment())
{
    //builder.UseDeveloperExceptionPage();
}

builder.Services.AddDbContext<AuthDbContext>(options =>
    options.UseNpgsql(
        builder.Configuration.GetConnectionString("DefaultConnection"))
);


// For Identity
builder.Services.AddIdentity<ApplicationUser, ApplicationUserRole>()
    .AddEntityFrameworkStores<AuthDbContext>()
    .AddDefaultTokenProviders();

//!_! ------------------ Configure
builder.Services.Configure<JwtSetting>(builder.Configuration.GetSection("Jwt"));
builder.Services.Configure<SecretSetting>(builder.Configuration.GetSection("Secret"));
builder.Services.Configure<RefreshTokenServiceSetting>(builder.Configuration.GetSection("RefreshTokenService"));


builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddAuthentication(option =>
{
    option.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    option.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    option.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;

}).AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateLifetime = true,
        ValidateAudience = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = builder.Configuration["Jwt:Issuer"],
        ValidAudience = builder.Configuration["Jwt:Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]))
    };
});

//!_! ------------------ Mapping Profiles
builder.Services.AddAutoMapper(cfg => cfg.AddProfile<MappingProfile>());
builder.Services.AddAutoMapper(cfg => cfg.AddProfile<CoreMapperProfile>());


//builder.Services.AddCors(x => x.AddDefaultPolicy(xx => { xx.AllowAnyOrigin(); xx.AllowAnyHeader(); }));


builder.Services.AddScoped<UserLogic>();
builder.Services.AddScoped<GenerateTokenHelper>();
builder.Services.AddScoped<PasswordHashHelper>();
builder.Services.AddScoped<IRefreshStorage, RefreshStorage>();


builder.Services.AddHostedService<RemoveOldRefreshTokenService>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    app.UseDeveloperExceptionPage();
}

app.UseHttpsRedirection();

app.UseCors();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;

    var context = services.GetRequiredService<AuthDbContext>();
    if (context.Database.GetPendingMigrations().Any())
    {
        context.Database.Migrate();
    }
}

app.Run();