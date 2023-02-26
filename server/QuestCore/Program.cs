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

var builder = WebApplication.CreateBuilder(args);

//����������� ������ ��� ��������� ������ �� ���������
builder.Services.Configure<ApiBehaviorOptions>(options =>
{
    options.SuppressModelStateInvalidFilter = true;
});


builder.Services.AddControllers().AddJsonOptions(options =>
{
    options.AllowInputFormatterExceptionMessages = true;
    options.JsonSerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
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
        Description = "JWT Authorization header using the Bearer scheme. \r\n\r\n Enter 'Bearer' [space] and then your token in the text input below.\r\n\r\nExample: \"Bearer 12345abcdef\"",
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
        //����������� ��������� ����� ������
        //�.�. ���������� �������� "� �����������"
        LifetimeValidator = QuestCoreLifetimeValidator.CheckLifeTime
    };
});


//builder.Services.AddScoped<ISmallEntitiesOperations, SmallEntitiesOperations>();

////!_! ------------------ Mapping Profiles
//builder.Services.AddAutoMapper(cfg => cfg.AddProfile<AuthMappingProfile>());


builder.Services.AddCors(x => x.AddDefaultPolicy(xx => { xx.AllowAnyOrigin(); xx.AllowAnyHeader(); }));

//������������� ������������ refit
//����������� ��� �������� � camelCase
var jsonSerializeOptions = new JsonSerializerOptions()
{
    PropertyNameCaseInsensitive = false,
    PropertyNamingPolicy = JsonNamingPolicy.CamelCase
};
jsonSerializeOptions.Converters.Add(new StageJsonConverterHelper<Stage>());

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


app.MapControllers();
app.Run();