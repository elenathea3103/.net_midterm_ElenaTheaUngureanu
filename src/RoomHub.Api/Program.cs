using RoomHub.Api.Features;
using RoomHub.Api.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddProblemDetails();
builder.Services.AddSingleton(TimeProvider.System);
builder.Services.AddInfrastructure(builder.Configuration, builder.Environment);
builder.Services.AddFeatures();

var app = builder.Build();

app.EnsureDatabaseCreated();

app.UseExceptionHandler(new ExceptionHandlerOptions
{
    StatusCodeSelector = exception => exception is BadHttpRequestException badRequest
        ? badRequest.StatusCode
        : StatusCodes.Status500InternalServerError
});
app.UseStatusCodePages();

app.MapGet("/", () => "RoomHub API is running.");
app.MapFeatures();

app.Run();
