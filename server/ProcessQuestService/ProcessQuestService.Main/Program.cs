using GenerateQuestsService.DataContracts.Interfaces;
using GenerateQuestsService.DataContracts.JsonHelpers;
using ProcessQuestService.Core.MappingProfiles;
using Refit;
using System.Text.Json;
using ProcessQuestService.Core.BusinessLogic;
using ProcessQuestService.Core.Helpers;
using ProcessQuestService.Core.HelperModels;
using ProcessQuestDataContracts.JsonHelpers;
using AuthService.DataContracts.Interfaces;
using ProcessQuestService.Core.Hubs;
using ProcessQuestService.ProcessQuestDatabase.Interfaces;
using ProcessQuestService.ProcessQuestDatabase.Implements;
using ProcessQuestService.ProcessQuestDatabase;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

//����������� ������ ��� ��������� ������ �� ���������
//builder.Services.Configure<ApiBehaviorOptions>(options =>
//{
//    options.SuppressModelStateInvalidFilter = true;
//});


//���������� Postgesql
builder.Services.AddDbContext<ProcessQuestContext>(options =>
    options
    .UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection"))
);

//��������� ������������
var redisSettings = builder.Configuration.GetSection(nameof(RedisSetting)).Get<RedisSetting>();

// ���������� �����������
builder.Services.AddStackExchangeRedisCache(options => {
    options.Configuration = redisSettings.Host;
    //options.InstanceName = redisSettings.InstanceName;
});

//Configure
builder.Services.Configure<RedisSetting>(builder.Configuration.GetSection(nameof(RedisSetting)));
builder.Services.Configure<JwtSetting>(builder.Configuration.GetSection("Jwt"));

//builder.Services.AddAuthorization();

builder.Services.AddAuthentication(options =>
{
    options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultForbidScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultSignInScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultSignOutScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateLifetime = false,
        ValidateAudience = true,
        ValidateIssuer = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = builder.Configuration["Jwt:Issuer"],
        ValidAudience = builder.Configuration["Jwt:Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]))
    };
    options.Events = new JwtBearerEvents
    {
        OnMessageReceived = context =>
        {
            var accessToken = context.Request.Query["access_token"];

            // если запрос направлен хабу
            var path = context.HttpContext.Request.Path;
            if (!string.IsNullOrEmpty(accessToken) && path.StartsWithSegments("/process"))
            {
                // получаем токен из строки запроса
                context.Token = accessToken;
            }
            return Task.CompletedTask;
        }
    };
});


//add socket hubs

builder.Services.AddSignalR();

//������ � ������������� �� 

builder.Services.AddScoped<ConnectQuestLogic>();
builder.Services.AddScoped<ProcessQuestLogic>();
builder.Services.AddScoped<CheckQuestLogic>();
builder.Services.AddScoped<ProcessQuestCacheHelper>();
builder.Services.AddScoped<ProcessQuestService.Core.Helpers.QuestJsonSerializer>();

//
builder.Services.AddScoped<ProcessQuestService.ProcessQuestDatabase.Helpers.QuestJsonSerializer>();
builder.Services.AddScoped<IProcessQuestStorage, ProcessQuestStorage>();
builder.Services.AddScoped<IProcessCacheStorage, ProcessCacheStorage>();
builder.Services.AddScoped<ProcessIdentityManager>();
                          

builder.Services.AddControllers().AddJsonOptions(options =>
{
    options.AllowInputFormatterExceptionMessages = true;
    options.JsonSerializerOptions.SetQuestJsonSerializerOptions();
    options.JsonSerializerOptions.SetProcessQuestJsonSerializerOptions();
});


//��������� Auto mapper
builder.Services.AddAutoMapper(cfg => cfg.AddProfile<ProcessQuestMappingProfile>());

//
builder.Services.AddCors(x => x.AddDefaultPolicy(xx => { xx.AllowAnyOrigin(); xx.AllowAnyHeader(); }));

builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(
        builder =>
        {
            builder.WithOrigins("http://127.0.0.1:5500")
                .AllowAnyHeader()
                .WithMethods("GET", "POST")
                .AllowCredentials();
        });
});

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
//builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

//����������� Refit
var refitJsonSerializerOptions = new JsonSerializerOptions
{
    PropertyNameCaseInsensitive = true
};
refitJsonSerializerOptions.SetQuestJsonSerializerOptions();
//������ ����� ��������� ��� ����� ��������
// "Value" = "value"
var refitSettings = new RefitSettings
{
    ContentSerializer = new SystemTextJsonContentSerializer(
        refitJsonSerializerOptions
    )
};


//����������� Refit �������
//!_! ------------------ Generate Quest Service
var generateQuestAddress = new Uri(builder.Configuration["GenerateQuestSettings:BaseAddress"]);
builder.Services.AddRefitClient<IGenerateQuestsApi>(refitSettings)
    .ConfigureHttpClient(c => c.BaseAddress = generateQuestAddress);

//!_! ------------------ Auth
var authAddress = new Uri(builder.Configuration["AuthSettings:BaseAddress"]);
builder.Services.AddRefitClient<IAuthApi>(refitSettings)
    .ConfigureHttpClient(c => c.BaseAddress = authAddress);


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment() || 1 == 1)
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors();
app.UseAuthentication();
app.UseAuthorization();


//app.UseHttpsRedirection();;

app.MapControllers();

var webSocketOptions = new WebSocketOptions
{
    KeepAliveInterval = TimeSpan.FromSeconds(5)
};
app.UseWebSockets(webSocketOptions);

app.MapHub<ProcessHub>("/process");

app.Run();
