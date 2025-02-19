using DatingTelegramBot.Errors;
using DatingTelegramBot.Extensions;
using Serilog;

var builder = WebApplication.CreateBuilder(args);
builder.Host.UseSerilog((context, configuration) => 
    configuration.ReadFrom.Configuration(context.Configuration));
var config = builder.Configuration;

builder.Services.AddControllers();
builder.Services.AddTelegramBot();
builder.Services.AddDbContext(config);
builder.Services.AddMemoryCache();
builder.Services.ConfigureTelegramBotMvc();
builder.Services.AddHelpers();
builder.Services.AddExceptionHandler<GlobalExceptionHandler>();
builder.Services.AddProblemDetails();
builder.Services.AddOptions(config);

var app = builder.Build();

app.UseExceptionHandler();

app.UseSerilogRequestLogging();

app.MapControllers();

app.Run();
