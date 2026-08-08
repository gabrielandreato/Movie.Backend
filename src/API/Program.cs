using API.Configuration;
using API.Middlewares;
using Infra.Ioc;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddConfiguration(builder.Configuration);

builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
    });

builder.Services.AddOpenApi();
builder.Services.AddInfraIoc(builder.Configuration);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();

    const string jsonPath = "/openapi/v1.json";

    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint(jsonPath, "OpenAPI v1");
    });
}

app.UseMiddleware<ExceptionHandlingMiddleware>();

app.UseHttpsRedirection();

app.UseCors(ServiceExtensions.CorsPolicyName);

app.UseAuthorization();

app.MapControllers();

app.Run();

public partial class Program
{
}
