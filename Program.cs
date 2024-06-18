using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;
using TestWebAPI.Config;
using TestWebAPI.Data;
using TestWebAPI.Helpers;
using TestWebAPI.Middlewares;
using TestWebAPI.Repositories;
using TestWebAPI.Repositories.Interfaces;
using TestWebAPI.Services;
using TestWebAPI.Services.Interfaces;

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
builder.Services.Configure<TokenSettings>(jwtSection);

var jwtSettings = jwtSection.Get<TokenSettings>();

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
        policy.Requirements.Add(new AuthorizationConfig("add-role")));
    options.AddPolicy("get-role", policy =>
        policy.Requirements.Add(new AuthorizationConfig("get-role")));
    options.AddPolicy("get-only-role", policy =>
        policy.Requirements.Add(new AuthorizationConfig("get-only-role")));
    options.AddPolicy("update-role", policy =>
        policy.Requirements.Add(new AuthorizationConfig("update-role")));
    options.AddPolicy("delete-role", policy =>
        policy.Requirements.Add(new AuthorizationConfig("delete-role")));
    options.AddPolicy("role-has-permission", policy =>
        policy.Requirements.Add(new AuthorizationConfig("role-has-permission")));

    // Policy permission
    options.AddPolicy("add-permission", policy =>
    policy.Requirements.Add(new AuthorizationConfig("add-permission")));
    options.AddPolicy("get-permission", policy =>
        policy.Requirements.Add(new AuthorizationConfig("get-permission")));
    options.AddPolicy("update-permissio", policy =>
        policy.Requirements.Add(new AuthorizationConfig("update-permission")));
    options.AddPolicy("delete-permission", policy =>
        policy.Requirements.Add(new AuthorizationConfig("delete-permission")));
});

// AutoMapper
#region Auto mapper
builder.Services.AddSingleton(provider => new MapperConfiguration(options =>
{
    options.AddProfile(new ApplicationMapper());
}).CreateMapper());
#endregion


builder.Services.Configure<EmailConfiguration>(builder.Configuration.GetSection("EmailConfiguration"));

// Add Repositories to the container.
builder.Services.AddScoped<IRoleRepositories, RoleRepositories>();
builder.Services.AddScoped<IAuthRepositories, AuthRepositories>();
builder.Services.AddScoped<IJwtRepositories, JwtRepositories>();
builder.Services.AddScoped<IPermisstionRepositories, PermisstionRepositories>();
builder.Services.AddScoped<IRoleHasPermissionRepositories, RoleHasPermissionRepositories>();
builder.Services.AddScoped<IUserRepositories, UserRepositories>();

// Add services to the container.
builder.Services.AddScoped<IRoleService, RoleService>();
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IJwtService, JwtService>();
builder.Services.AddScoped<IPermissionServices, PermissionServices>();
builder.Services.AddScoped<ISendMailService, SendMailServices>();
builder.Services.AddScoped<IRoleHasPermissionServices, RoleHasPermissionServices>();
builder.Services.AddScoped<IUserServices, UserServices>();

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
