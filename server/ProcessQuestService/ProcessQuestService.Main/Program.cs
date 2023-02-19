using GenerateQuestsService.DataContracts.Interfaces;
using GenerateQuestsService.DataContracts.JsonHelpers;
using ProcessQuestService.Core.MappingProfiles;
using Refit;
using System.Text.Json;
using ProcessQuestService.Core.BusinessLogic;
using ProcessQuestService.Core.Helpers;
using ProcessQuestService.Core.HelperModels;

var builder = WebApplication.CreateBuilder(args);

//подавляется фильтр для валидации модели по умолчанию
//builder.Services.Configure<ApiBehaviorOptions>(options =>
//{
//    options.SuppressModelStateInvalidFilter = true;
//});


//используем Postgesql
//builder.Services.AddDbContext<QuestContext>(options =>
//    options
//    .UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection"))
//);

//добавляем конфигурации
var redisSettings = builder.Configuration.GetSection(nameof(RedisSetting)).Get<RedisSetting>();

// добавление кэширования
builder.Services.AddStackExchangeRedisCache(options => {
    options.Configuration = redisSettings.Host;
    options.InstanceName = redisSettings.InstanceName;
});


builder.Services.Configure<RedisSetting>(builder.Configuration.GetSection(nameof(RedisSetting)));

//раздел с конфигурацией ЖЦ 

builder.Services.AddScoped<ConnectQuestLogic>();
builder.Services.AddScoped<ProcessQuestLogic>();
builder.Services.AddScoped<ProcessQuestCacheHelper>();

//добавляем контроллеры и конфигурируем Json опции при десериализации моделей
//добавляем собственный полиморфный десериализатор
//и десериализатор енамов
builder.Services.AddControllers().AddJsonOptions(options =>
{
    options.AllowInputFormatterExceptionMessages = true;
    options.JsonSerializerOptions.SetQuestJsonSerializerOptions();
});

//добавляем Auto mapper
builder.Services.AddAutoMapper(cfg => cfg.AddProfile<ProcessQuestMappingProfile>());



// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
//builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

//настраиваем Refit
//ставим опцию сравнения без учета регистра
// "Value" = "value"
var refitSettings = new RefitSettings
{
    ContentSerializer = new SystemTextJsonContentSerializer(
        new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        }
    )
};


//подключаемм Refit клиенты
//!_! ------------------ Generate Quest Service
var generateQuestAddress = new Uri(builder.Configuration["GenerateQuestSettings:BaseAddress"]);
builder.Services.AddRefitClient<IGenerateQuestsApi>(refitSettings)
    .ConfigureHttpClient(c => c.BaseAddress = generateQuestAddress);


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment() || 1 == 1)
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.MapControllers();

var webSocketOptions = new WebSocketOptions
{
    KeepAliveInterval = TimeSpan.FromSeconds(5)
};
app.UseWebSockets(webSocketOptions);

app.Run();
