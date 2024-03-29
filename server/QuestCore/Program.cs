using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;
using System.Text.Json;
using AuthService.DataContracts.Interfaces;
using Refit;
using GenerateQuestsService.DataContracts.Interfaces;
using GenerateQuestsService.DataContracts.Models.Stages;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System;
using QuestCore.TokenHelpers;
using GenerateQuestsService.DataContracts.JsonHelpers;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Rewrite;
using QuestCore.HelperModels;
using QuestCore.Helpers;
using ProcessQuestDataContracts.Interfaces;
using ProcessQuestDataContracts.JsonHelpers;
using ProcessQuestDataContracts.Models.Stages;

var builder = WebApplication.CreateBuilder(args);

//����������� ������ ��� ��������� ������ �� ���������
builder.Services.Configure<ApiBehaviorOptions>(options =>
{
    options.SuppressModelStateInvalidFilter = true;
});

builder.Services.Configure<ProcessQuestSettings>(builder.Configuration.GetSection("ProcessQuestSettings"));

builder.Services.AddControllers().AddJsonOptions(options =>
{
    options.AllowInputFormatterExceptionMessages = true;
    options.JsonSerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
    options.JsonSerializerOptions.Converters.Add(new ProcessStageJsonConverterHelper<StageProcess>());
    options.JsonSerializerOptions.Converters.Add(new StageJsonConverterHelper<Stage>());
    options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter(JsonNamingPolicy.CamelCase));
   // options.JsonSerializerOptions.PropertyNameCaseInsensitive = true;
});

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(swagger =>
{
    swagger.SwaggerDoc("v1", new OpenApiInfo { Title = "QuestCore", Version = "v1" });

    swagger.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme()
    {
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "JWT Authorization header using the Bearer scheme. \r\n\r\n Enter into input token value without quotes.\r\n\r\nExample: 2cdfcdf342342344232",
    });
    swagger.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            new string[] {}
        }
    });
});

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
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"])),
        LifetimeValidator = QuestCoreLifetimeValidator.CheckLifeTime
    };
});


//builder.Services.AddScoped<ISmallEntitiesOperations, SmallEntitiesOperations>();

////!_! ------------------ Mapping Profiles
//builder.Services.AddAutoMapper(cfg => cfg.AddProfile<AuthMappingProfile>());


builder.Services.AddCors(x => x.AddDefaultPolicy(xx => { xx.WithOrigins("*").AllowAnyHeader().AllowAnyMethod(); }));


//настраиваем переадресацию 
//var options = new RewriteOptions()
//    .Add(RewriteSocketHelper.RewriteSocketRequests);
    //.AddRewrite("wss://localhost:44393/room/(.*)", builder.Configuration["ProcessQuestSettings:SocketAddress"], skipRemainingRules: false);



//������������� ������������ refit
//����������� ��� �������� � camelCase
var jsonSerializeOptions = new JsonSerializerOptions()
{
    PropertyNameCaseInsensitive = false,
    PropertyNamingPolicy = JsonNamingPolicy.CamelCase
};
jsonSerializeOptions.Converters.Add(new ProcessStageJsonConverterHelper<StageProcess>());
jsonSerializeOptions.Converters.Add(new StageJsonConverterHelper<Stage>());
//тут был баг с созданием квеста
//jsonSerializeOptions.Converters.Add(new JsonStringEnumConverter(JsonNamingPolicy.CamelCase));

var refitSettings = new RefitSettings
{
    ContentSerializer = new SystemTextJsonContentSerializer(jsonSerializeOptions),
};

//!_! ------------------ Auth
var authAddress = new Uri(builder.Configuration["AuthSettings:BaseAddress"]);
builder.Services.AddRefitClient<IAuthApi>(refitSettings)
    .ConfigureHttpClient(c => c.BaseAddress = authAddress);

//!_! ------------------ Generate Quest Service
var generateQuestAddress = new Uri(builder.Configuration["GenerateQuestSettings:BaseAddress"]);
builder.Services.AddRefitClient<IGenerateQuestsApi>(refitSettings)
    .ConfigureHttpClient(c => c.BaseAddress = generateQuestAddress);

//!_! ------------------ Process Quest Service
var processQuestAddress = new Uri(builder.Configuration["ProcessQuestSettings:BaseAddress"]);
builder.Services.AddRefitClient<IProcessQuestApi>(refitSettings)
    .ConfigureHttpClient(c => c.BaseAddress = processQuestAddress);


var app = builder.Build();


if (app.Environment.IsDevelopment() || 1 == 1)
{
    app.UseSwagger();
    app.UseSwaggerUI();
    app.UseDeveloperExceptionPage();
}

//app.UseHttpsRedirection();

app.UseCors();

app.UseAuthentication();
app.UseAuthorization();

//app.UseRewriter(options);

app.MapControllers();
app.Run();