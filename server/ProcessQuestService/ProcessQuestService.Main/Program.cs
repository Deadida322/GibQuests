using GenerateQuestsService.DataContracts.Interfaces;
using GenerateQuestsService.DataContracts.JsonHelpers;
using ProcessQuestService.Core.MappingProfiles;
using Refit;
using System.Text.Json;
using ProcessQuestService.Core.BusinessLogic;
using ProcessQuestService.Core.Helpers;
using ProcessQuestService.Core.HelperModels;
using GenerateQuestsService.DataContracts.Models.Stages;
using ProcessQuestDataContracts;
using ProcessQuestDataContracts.Models.Stages;
using ProcessQuestDataContracts.JsonHelpers;
using AuthService.DataContracts.Interfaces;

var builder = WebApplication.CreateBuilder(args);

//����������� ������ ��� ��������� ������ �� ���������
//builder.Services.Configure<ApiBehaviorOptions>(options =>
//{
//    options.SuppressModelStateInvalidFilter = true;
//});


//���������� Postgesql
//builder.Services.AddDbContext<QuestContext>(options =>
//    options
//    .UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection"))
//);

//��������� ������������
var redisSettings = builder.Configuration.GetSection(nameof(RedisSetting)).Get<RedisSetting>();

// ���������� �����������
builder.Services.AddStackExchangeRedisCache(options => {
    options.Configuration = redisSettings.Host;
    //options.InstanceName = redisSettings.InstanceName;
});


builder.Services.Configure<RedisSetting>(builder.Configuration.GetSection(nameof(RedisSetting)));

//������ � ������������� �� 

builder.Services.AddScoped<ConnectQuestLogic>();
builder.Services.AddScoped<ProcessQuestLogic>();
builder.Services.AddScoped<CheckQuestLogic>();
builder.Services.AddScoped<ProcessQuestCacheHelper>();
builder.Services.AddScoped<QuestJsonSerializer>();

//��������� ����������� � ������������� Json ����� ��� �������������� �������
//��������� ����������� ����������� ��������������
//� �������������� ������
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

//app.UseHttpsRedirection();;

app.MapControllers();

var webSocketOptions = new WebSocketOptions
{
    KeepAliveInterval = TimeSpan.FromSeconds(5)
};
app.UseWebSockets(webSocketOptions);

app.Run();
