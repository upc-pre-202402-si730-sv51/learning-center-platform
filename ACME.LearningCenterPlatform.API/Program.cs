using ACME.LearningCenterPlatform.API.IAM.Application.Internal.CommandServices;
using ACME.LearningCenterPlatform.API.IAM.Application.Internal.OutboundServices;
using ACME.LearningCenterPlatform.API.IAM.Application.Internal.QueryServices;
using ACME.LearningCenterPlatform.API.IAM.Domain.Repositories;
using ACME.LearningCenterPlatform.API.IAM.Domain.Services;
using ACME.LearningCenterPlatform.API.IAM.Infrastructure.Hashing.BCrypt.Services;
using ACME.LearningCenterPlatform.API.IAM.Infrastructure.Persistence.EFC.Repositories;
using ACME.LearningCenterPlatform.API.IAM.Infrastructure.Tokens.JWT.Configuration;
using ACME.LearningCenterPlatform.API.IAM.Infrastructure.Tokens.JWT.Services;
using ACME.LearningCenterPlatform.API.Profiles.Application.Internal.CommandServices;
using ACME.LearningCenterPlatform.API.Profiles.Application.Internal.QueryServices;
using ACME.LearningCenterPlatform.API.Profiles.Domain.Repositories;
using ACME.LearningCenterPlatform.API.Profiles.Domain.Services;
using ACME.LearningCenterPlatform.API.Profiles.Infrastructure.Persistence.EFC.Repositories;
using ACME.LearningCenterPlatform.API.Publishing.Application.Internal.CommandServices;
using ACME.LearningCenterPlatform.API.Publishing.Application.Internal.QueryServices;
using ACME.LearningCenterPlatform.API.Publishing.Domain.Repositories;
using ACME.LearningCenterPlatform.API.Publishing.Domain.Services;
using ACME.LearningCenterPlatform.API.Publishing.Infrastructure.Persistence.EFC.Repositories;
using ACME.LearningCenterPlatform.API.Shared.Domain.Repositories;
using ACME.LearningCenterPlatform.API.Shared.Infrastructure.Interfaces.ASP.Configuration;
using ACME.LearningCenterPlatform.API.Shared.Infrastructure.Persistence.EFC.Configuration;
using ACME.LearningCenterPlatform.API.Shared.Infrastructure.Persistence.EFC.Repositories;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddRouting(options => options.LowercaseUrls = true);
builder.Services.AddControllers(options => options.Conventions.Add(new KebabCaseRouteNamingConvention()));

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

if (connectionString == null)
{
    throw new InvalidOperationException("Connection string not found.");
}

builder.Services.AddDbContext<AppDbContext>(options =>
{
    if (builder.Environment.IsDevelopment())
    {

        options.UseMySQL(connectionString)
            .LogTo(Console.WriteLine, LogLevel.Information)
            .EnableSensitiveDataLogging()
            .EnableDetailedErrors();
    }
    else if (builder.Environment.IsProduction())
    {
        options.UseMySQL(connectionString)
            .LogTo(Console.WriteLine, LogLevel.Error);
    }
});

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options => options.EnableAnnotations());

// Dependency Injection

// Shared Bounded Context
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

// Publishing Bounded Context
builder.Services.AddScoped<ICategoryRepository, CategoryRepository>();
builder.Services.AddScoped<ITutorialRepository, TutorialRepository>();
builder.Services.AddScoped<ICategoryCommandService, CategoryCommandService>();
builder.Services.AddScoped<ICategoryQueryService, CategoryQueryService>();
builder.Services.AddScoped<ITutorialCommandService, TutorialCommandService>();
builder.Services.AddScoped<ITutorialQueryService, TutorialQueryService>();

// Profiles Bounded Context Dependency Injection Configuration
builder.Services.AddScoped<IProfileRepository, ProfileRepository>();
builder.Services.AddScoped<IProfileCommandService, ProfileCommandService>();
builder.Services.AddScoped<IProfileQueryService, ProfileQueryService>();

// IAM Bounded Context Dependency Injection Configuration

// TokenSettings Configuration

builder.Services.Configure<TokenSettings>(builder.Configuration.GetSection("TokenSettings"));

builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IUserCommandService, UserCommandService>();
builder.Services.AddScoped<IUserQueryService, UserQueryService>();
builder.Services.AddScoped<ITokenService, TokenService>();
builder.Services.AddScoped<IHashingService, HashingService>();

var app = builder.Build();

// Verify if the database exists and create it if it doesn't
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    var context = services.GetRequiredService<AppDbContext>();

    context.Database.EnsureCreated();
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();