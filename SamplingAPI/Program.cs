using Microsoft.OpenApi.Models;
using SamplingAPI.Services;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddScoped<IEstimationService, EstimationService>();
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo()
    {
        Title = "Sampling API",
        Description = "An API to provide useful tools for sampling",
        Version = "v1"
    });
    string xmlFilename = $"{Assembly.GetExecutingAssembly().Location}.xml";
});

var app = builder.Build();

if (app.Environment.IsDevelopment() || app.Configuration.GetValue("USE_SWAGGER_UI", false))
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();

public partial class Program { }
