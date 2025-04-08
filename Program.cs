using EventManagement.API.Middleware; // Add middleware namespace
using EventManagement.Application.Interfaces.Persistence;
using EventManagement.Application.Interfaces.Services; // Add service interface namespace
using EventManagement.Application.Mappings;
using EventManagement.Application.Services; // Add service implementation namespace
using EventManagement.Infrastructure.Persistence;
using EventManagement.Infrastructure.Persistence.Repositories;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

// Configure DbContext
var connectionString = builder.Configuration.GetConnectionString("MariaDbConnection");
if (string.IsNullOrEmpty(connectionString))
{
    throw new InvalidOperationException("Connection string 'MariaDbConnection' not found.");
}
builder.Services.AddDbContext<EventManagementDbContext>(options =>
    options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString)));
// Register Repositories
builder.Services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>)); // Register generic repository
builder.Services.AddScoped<IEventRepository, EventRepository>();
builder.Services.AddScoped<ILocationRepository, LocationRepository>();
builder.Services.AddScoped<IRoomRepository, RoomRepository>();
builder.Services.AddScoped<ICategoryRepository, CategoryRepository>();
builder.Services.AddScoped<IParticipantRepository, ParticipantRepository>();
builder.Services.AddScoped<ISpeakerRepository, SpeakerRepository>();
builder.Services.AddScoped<ISessionRepository, SessionRepository>();
builder.Services.AddScoped<IRatingRepository, RatingRepository>();
// Register UnitOfWork
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
// Register AutoMapper
builder.Services.AddAutoMapper(typeof(MappingProfile).Assembly); // Scan assembly containing MappingProfile
// Register Application Services
builder.Services.AddScoped<IEventService, EventService>();
builder.Services.AddScoped<IParticipantService, ParticipantService>();
builder.Services.AddScoped<ICategoryService, CategoryService>();
builder.Services.AddScoped<IRoomService, RoomService>();
builder.Services.AddScoped<ISessionService, SessionService>();
builder.Services.AddScoped<ISpeakerService, SpeakerService>(); // Register SpeakerService
builder.Services.AddScoped<IRatingService, RatingService>(); // Register RatingService

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
app.UseSwagger();
app.UseSwaggerUI();

// Add global error handling middleware
app.UseGlobalErrorHandling();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
