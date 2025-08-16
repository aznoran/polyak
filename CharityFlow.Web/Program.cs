using CharityFlow.Application.Repositories;
using CharityFlow.Application.Services;
using CharityFlow.Infrastructure.Repositories;
using CharityFlow.Infrastructure.Services;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

// Настройка Serilog
Log.Logger = new LoggerConfiguration()
    .WriteTo.Console()
    .CreateLogger();

builder.Host.UseSerilog();

// Добавление сервисов
builder.Services.AddSerilog();
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Настройка MediatR
builder.Services.AddMediatR(cfg => 
{
    cfg.RegisterServicesFromAssembly(typeof(CharityFlow.Application.Commands.UpdateProjectCommand).Assembly);
    cfg.RegisterServicesFromAssembly(typeof(CharityFlow.Infrastructure.Services.NotificationService).Assembly);
});

// Регистрация зависимостей
builder.Services.AddSingleton<IProjectRepository, ProjectRepository>();
builder.Services.AddScoped<INotificationService, NotificationService>();

var app = builder.Build();

// Настройка pipeline
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

try
{
    Log.Information("Запуск CharityFlow API...");
    app.Run();
}
catch (Exception ex)
{
    Log.Fatal(ex, "Ошибка при запуске приложения");
}
finally
{
    Log.CloseAndFlush();
}