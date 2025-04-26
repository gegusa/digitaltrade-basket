using DigitalTrade.Basket.AppServices;
using DigitalTrade.Basket.Entities;

var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

var configuration = builder.Configuration;

builder.Services.AddMvc();
builder.Services.AddControllers();

builder.Services.AddEntities(configuration);
builder.Services.AddAppServices(configuration);

builder.Services.AddCors();
builder.Services.AddHttpContextAccessor();
builder.Services.AddHealthChecks();
builder.Services.AddEndpointsApiExplorer();

app.MapGet("/", () => "Hello World!");

app.Run();