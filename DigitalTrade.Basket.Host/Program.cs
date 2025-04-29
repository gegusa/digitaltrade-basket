using DigitalTrade.Basket.AppServices;
using DigitalTrade.Basket.Entities;
using DigitalTrade.Basket.Host.Extensions;
using DigitalTrade.Basket.Host.Options;
using DigitalTrade.Catalog.Api.Contracts.Catalog.Web;
using Scalar.AspNetCore;
using static DigitalTrade.Basket.Host.Extensions.ServiceCollectionExtensions;

var builder = WebApplication.CreateBuilder(args);
var configuration = builder.Configuration;

builder.Services.AddMvc();
builder.Services.AddControllers();

builder.Services.AddEntities(configuration);
builder.Services.AddAppServices(configuration);
builder.Services.AddKafkaFlow(configuration);

builder.Services.AddCors();
builder.Services.AddHttpContextAccessor();
builder.Services.AddHealthChecks();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddOpenApi();

AddHttpClientFor<ICatalogApi, CatalogHttpOptions>(builder.Services, configuration);

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference();
}
app.MapControllers();

app.Run();