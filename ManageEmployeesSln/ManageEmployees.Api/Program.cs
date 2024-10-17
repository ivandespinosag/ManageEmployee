using AutoMapper;
using ManageEmployees.Api.Middleware;
using ManageEmployees.Core.Handlers;
using ManageEmployees.Core.Interfaces;
using ManageEmployees.Core.Utilities;
using ManageEmployees.Infraestructure;
using ManageEmployees.Infraestructure.Data;
using ManageEmployees.Infraestructure.Interfaces;
using ManageEmployees.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.Configure<ConnectionStrings>(builder.Configuration.GetSection("ConnectionStrings"));

// Configure SMTP
builder.Services.Configure<SmtpSettings>(builder.Configuration.GetSection("SmtpSettings"));

// Register your service implementation

var mapperConfig = new MapperConfiguration(mc =>
{
    mc.AddProfile(new AutoMapperProfiles());
});

IMapper mapper = mapperConfig.CreateMapper();
builder.Services.AddSingleton(mapper);

builder.Services.AddSingleton<IEmployeeData, EmployeeData>();
builder.Services.AddSingleton<IEmployeeHandler, EmployeeHandler>();
builder.Services.AddSingleton<INotificationService, NotificationService>();

// Configure CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAnyOrigin",
        builder => builder
            .AllowAnyOrigin()
            .AllowAnyMethod()
            .AllowAnyHeader());
});

// HostedService, background worker
builder.Services.AddHostedService<NotificationHostedService>();

var app = builder.Build();

app.UseMiddleware<ExceptionMiddleware>();

// Enable CORS
app.UseCors("AllowAnyOrigin");

// Configure the HTTP request pipeline.
if (app.Environment.IsProduction() || app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
