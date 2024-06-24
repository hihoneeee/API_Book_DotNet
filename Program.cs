using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;
using TestWebAPI.Configs;
using TestWebAPI.Data;
using TestWebAPI.Helpers;
using TestWebAPI.Middlewares;
using TestWebAPI.Repositories;
using TestWebAPI.Repositories.Interfaces;
using TestWebAPI.Services;
using TestWebAPI.Services.Interfaces;
using TestWebAPI.Settings;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();


// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle

// config swapgger token
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = @"JWT Authorization header using the Bearer scheme. \r\n\r\n 
                      Enter your token in the text input below.
                      \r\n\r\nExample: '12345abcdef'",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer"
    });

    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
          new OpenApiSecurityScheme
          {
            Reference = new OpenApiReference
            {
                Type = ReferenceType.SecurityScheme,
                Id = "Bearer"
            },
            Scheme = "Bearer",
            Name = "Bearer",
            In = ParameterLocation.Header,
          },
          new List<string>()
        }
    });

    c.OperationFilter<AddBearerTokenConfig>();
});


// Set Cors
builder.Services.AddCors(opt => opt.AddDefaultPolicy(policy =>
    policy.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod()));

// Connect DB
builder.Services.AddDbContext<ApplicationDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("GoodRest"));
});


// JWT config
var jwtSection = builder.Configuration.GetSection("AppSettings");
builder.Services.Configure<TokenSetting>(jwtSection);

var jwtSettings = jwtSection.Get<TokenSetting>();

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.RequireHttpsMetadata = false;
        options.SaveToken = false;
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = false,
            ValidateAudience = false,
            ValidateLifetime = true,
            ClockSkew = TimeSpan.Zero,
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings.Secret)),
            RoleClaimType = "roleCode"
        };
        options.Events = new JwtBearerEvents
        {
            OnMessageReceived = context =>
            {
                return Task.CompletedTask;
            },
            OnChallenge = context =>
            {
                // Override the response status code and message
                context.HandleResponse();
                context.Response.StatusCode = 401;
                context.Response.ContentType = "application/json";
                var result = Newtonsoft.Json.JsonConvert.SerializeObject(new
                {
                    Success = "false",
                    message = "Token Invalid"
                });
                return context.Response.WriteAsync(result);
            }
        };

    });

//config permission 
builder.Services.AddScoped<IAuthorizationHandler, PermissionHandler>();
builder.Services.AddAuthorization(options =>
{
    // Policy role
    options.AddPolicy("add-role", policy =>
        policy.Requirements.Add(new AuthorizationSetting("add-role")));
    options.AddPolicy("get-role", policy =>
        policy.Requirements.Add(new AuthorizationSetting("get-role")));
    options.AddPolicy("get-only-role", policy =>
        policy.Requirements.Add(new AuthorizationSetting("get-only-role")));
    options.AddPolicy("update-role", policy =>
        policy.Requirements.Add(new AuthorizationSetting("update-role")));
    options.AddPolicy("delete-role", policy =>
        policy.Requirements.Add(new AuthorizationSetting("delete-role")));
    options.AddPolicy("assign-permission", policy =>
        policy.Requirements.Add(new AuthorizationSetting("assign-permission")));

    // Policy permission
    options.AddPolicy("add-permission", policy =>
    policy.Requirements.Add(new AuthorizationSetting("add-permission")));
    options.AddPolicy("get-permission", policy =>
        policy.Requirements.Add(new AuthorizationSetting("get-permission")));
    options.AddPolicy("update-permissio", policy =>
        policy.Requirements.Add(new AuthorizationSetting("update-permission")));
    options.AddPolicy("delete-permission", policy =>
        policy.Requirements.Add(new AuthorizationSetting("delete-permission")));

    // Policy category
    options.AddPolicy("add-category", policy =>
    policy.Requirements.Add(new AuthorizationSetting("add-category")));
    options.AddPolicy("get-category", policy =>
        policy.Requirements.Add(new AuthorizationSetting("get-category")));
    options.AddPolicy("update-category", policy =>
        policy.Requirements.Add(new AuthorizationSetting("update-category")));
    options.AddPolicy("delete-category", policy =>
        policy.Requirements.Add(new AuthorizationSetting("delete-category")));
});

// AutoMapper
#region Auto mapper
builder.Services.AddSingleton(provider => new MapperConfiguration(options =>
{
    options.AddProfile(new ApplicationMapper());
}).CreateMapper());
#endregion

//builder settings
builder.Services.Configure<SendEmailSetting>(builder.Configuration.GetSection("SendEmailSetting"));
builder.Services.Configure<CloudinarySetting>(builder.Configuration.GetSection("CloudinarySetting"));

// Add Repositories to the container.
builder.Services.AddScoped<IRoleRepositories, RoleRepositories>();
builder.Services.AddScoped<IAuthRepositories, AuthRepositories>();
builder.Services.AddScoped<IJwtRepositories, JwtRepositories>();
builder.Services.AddScoped<IPermisstionRepositories, PermisstionRepositories>();
builder.Services.AddScoped<IRoleHasPermissionRepositories, RoleHasPermissionRepositories>();
builder.Services.AddScoped<IUserRepositories, UserRepositories>();
builder.Services.AddScoped<ICategoryRepositories, CategoryRepositories>();
builder.Services.AddScoped<IPropertyRepositories, PropertyRepositories>();
builder.Services.AddScoped<IPropertyHasDetailRepositories, PropertyHasDetailRepositories>();
builder.Services.AddScoped<FakeDataRepositories>();

// Add services to the container.
builder.Services.AddScoped<IRoleService, RoleServices>();
builder.Services.AddScoped<IAuthService, AuthServices>();
builder.Services.AddScoped<IJwtServices, JwtServices>();
builder.Services.AddScoped<IPermissionServices, PermissionServices>();
builder.Services.AddScoped<ISendMailServices, SendMailServices>();
builder.Services.AddScoped<IRoleHasPermissionServices, RoleHasPermissionServices>();
builder.Services.AddScoped<IUserServices, UserServices>();
builder.Services.AddScoped<ICategoryServices, CategoryServices>();
builder.Services.AddScoped<ICloudinaryServices, CloudinaryServices>();
builder.Services.AddScoped<IPropertyServices, PropertyServices>();
// Register JWTHelper
builder.Services.AddScoped<IJWTHelper, JWTHelper>();

//Htttp cookie
builder.Services.AddHttpContextAccessor();
var app = builder.Build();

app.UseExceptionHandler(errorApp =>
{
    errorApp.Run(async context =>
    {
        var exceptionHandlerPathFeature = context.Features.Get<IExceptionHandlerPathFeature>();
        var exception = exceptionHandlerPathFeature?.Error;

        // Handle exception here
        context.Response.StatusCode = 500;
        context.Response.ContentType = "application/json";
        var result = Newtonsoft.Json.JsonConvert.SerializeObject(new
        {
            Success = "false",
            message = "Internal Server Error"
        });
        await context.Response.WriteAsync(result);
    });
});


app.UseMiddleware<ErrorHandlingToken>();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseMiddleware<ErrorHandlingToken>();

app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();
app.Run();
