using Microsoft.EntityFrameworkCore;
using UrlShortner.Data.Models.Config;
using UrlShortner.Data.Persistence;
using UrlShortner.Data.Repositories.ApiKey;
using UrlShortner.Data.Repositories.ForgotKey;
using UrlShortner.Data.Repositories.Url;
using UrlShortner.Data.Repositories.User;
using UrlShortner.Data.Services.ApiKey;
using UrlShortner.Data.Services.Email;
using UrlShortner.Data.Services.Email.Config;
using UrlShortner.Data.Services.ForgotKey;
using UrlShortner.Data.Services.Url;
using UrlShortner.Server.Middlewares;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Configuration.AddEnvironmentVariables();
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<AppDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("UrlDB"));
});

builder.Services.AddScoped<IUrlRepository, UrlRepository>();
builder.Services.AddScoped<IUrlService, UrlService>();
builder.Services.AddScoped<IApiKeyService, ApiKeyService>();
builder.Services.AddScoped<IApiKeyRepository, ApiKeyRepository>();
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddTransient<IEmailService, EmailService>();
builder.Services.AddScoped<IForgotKeyRepository, ForgotKeyRepository>();
builder.Services.AddScoped<IForgotKeyService, ForgotKeyService>();

builder.Services.Configure<ApiSettings>(builder.Configuration.GetSection("ApiSettings"));

builder.Services.AddSwaggerGen(options =>
{
    options.AddSecurityDefinition("ApiKey", new Microsoft.OpenApi.Models.OpenApiSecurityScheme
    {
        Name = "X-Api-Key",
        Type = Microsoft.OpenApi.Models.SecuritySchemeType.ApiKey,
        In = Microsoft.OpenApi.Models.ParameterLocation.Header,
    });

    options.AddSecurityRequirement(new Microsoft.OpenApi.Models.OpenApiSecurityRequirement
    {
        {
            new Microsoft.OpenApi.Models.OpenApiSecurityScheme
            {
                Reference = new Microsoft.OpenApi.Models.OpenApiReference
                {
                    Type = Microsoft.OpenApi.Models.ReferenceType.SecurityScheme,
                    Id = "ApiKey"
                }
            },
            new string[] {}
        }
    });
});

#region SMTP Mail Config (Mailtrap.io)
var emailConfig = builder.Services.Configure<EmailConfig>(builder.Configuration.GetSection("MailConfig"));
#endregion

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseWhen(context => !context.Request.Path.StartsWithSegments("/api/apikey/generate-new-api-key") 
            && !context.Request.Path.StartsWithSegments("/api/apikey/get-api-key")
            && !context.Request.Path.StartsWithSegments("/api/apikey/revoke")
            && !context.Request.Path.StartsWithSegments("/api/apikey/user-generate-new-key")
            && !context.Request.Path.StartsWithSegments("/api/test/send-mail-test")
            && !context.Request.Path.StartsWithSegments("/api/apikey/forgot-key")
            && !context.Request.Path.StartsWithSegments("/api/apikey/change-key"),
    appBuilder => appBuilder.UseMiddleware<ApiKeyAuthMiddleware>()
);

app.UseAuthorization();

app.MapControllers();

app.Run();
