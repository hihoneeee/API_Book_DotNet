﻿using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;
using TestWebAPI.Configs;
using TestWebAPI.Data;
using TestWebAPI.Helpers;
using TestWebAPI.Helpers.IHelpers;
using TestWebAPI.Middlewares;
using TestWebAPI.Middlewares.Interfaces;
using TestWebAPI.Repositories;
using TestWebAPI.Repositories.Interfaces;
using TestWebAPI.Services;
using TestWebAPI.Services.Interfaces;
using TestWebAPI.Settings;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddSignalR();

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
var clientRootAddress = builder.Configuration["App:ClientRootAddress"];

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.WithOrigins(clientRootAddress)
              .AllowAnyHeader()
              .AllowAnyMethod()
              .AllowCredentials();
    });
});

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
                var accessToken = context.Request.Query["access_token"];
                var path = context.HttpContext.Request.Path;

                if (!string.IsNullOrEmpty(accessToken) && path.StartsWithSegments("/chathub"))
                {
                    context.Token = accessToken;
                }
                return Task.CompletedTask;
            },
            OnChallenge = context =>
            {
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
builder.Services.Configure<RedisCacheSetting>(builder.Configuration.GetSection("RedisCacheSetting"));
builder.Services.Configure<PaypalSetiing>(builder.Configuration.GetSection("PaypalSetting"));

// Register the setting
builder.Services.AddSingleton<RedisCacheConfig>(provider =>
{
    var redisConfig = provider.GetRequiredService<IOptions<RedisCacheSetting>>().Value;
    return new RedisCacheConfig(redisConfig.ConnectionString);
});

builder.Services.AddSingleton<PaypalConfig>(serviceProvider =>
{
    var paypalSettings = serviceProvider.GetRequiredService<IOptions<PaypalSetiing>>().Value;
    return new PaypalConfig(paypalSettings.appId, paypalSettings.appSecret, paypalSettings.mode);
});

// Add Repositories to the container
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
builder.Services.AddScoped<IChatHubRepositories, ChatHubRepositories>();
builder.Services.AddScoped<INotificationRepositories, NotificationRepositories>();
builder.Services.AddScoped<IAppointmentRepositories, AppointmentRepositories>();
builder.Services.AddScoped<IContractRepositories, ContractRepositories>();
builder.Services.AddScoped<IPaymentRepositories, PaymentRepositories>();

// Add services to the container
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
builder.Services.AddScoped<IPropertyHasDetailServices, PropertyHasDetailServices>();
builder.Services.AddScoped<IRealTimeServices, RealTimeServices>();
builder.Services.AddScoped<IAppointmentServices, AppointmentServices>();
builder.Services.AddScoped<IContractServices, ContractServices>();
builder.Services.AddScoped<IPaymentServices, PaymentServices>();
builder.Services.AddScoped<INotificationServices, NotificationServices>();
builder.Services.AddScoped<IConversationServices, ConversationServices>();

// Add middleware and Helper to the container
builder.Services.AddScoped<ICookieHelper, CookieHelper>();
builder.Services.AddScoped<IJWTHelper, JWTHelper>();
builder.Services.AddScoped<IHashPasswordHelper, HashPasswordHelper>();
builder.Services.AddScoped<ChatHub>();

//Htttp cookie
builder.Services.AddHttpContextAccessor();

// client
builder.Services.AddDirectoryBrowser();

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
    app.UseDeveloperExceptionPage();
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseRouting();

app.UseStaticFiles(new StaticFileOptions
{
    FileProvider = new PhysicalFileProvider(
        Path.Combine(Directory.GetCurrentDirectory(), "StaticFiles")),
    RequestPath = "/static"
});

app.UseHttpsRedirection();
app.UseCors("AllowAll");
app.UseAuthentication();
app.UseAuthorization();  

app.UseEndpoints(endpoints =>
{
    endpoints.MapControllers();
    endpoints.MapHub<ChatHub>("/chathub");
});
app.Run();

