using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using RoomHub.Api.Domain;
using RoomHub.Api.Domain.Bookings;
using RoomHub.Api.Domain.Rooms;
using RoomHub.Api.Infrastructure.Repositories;

namespace RoomHub.Api.Infrastructure;

public static class InfrastructureExtensions
{
    private const string ConnectionStringName = "RoomHub";

    public static IServiceCollection AddInfrastructure(
        this IServiceCollection services,
        IConfiguration configuration,
        IHostEnvironment environment)
    {
        var connectionString = configuration.GetConnectionString(ConnectionStringName)
            ?? throw new InvalidOperationException($"Connection string '{ConnectionStringName}' is missing.");

        var sqlite = new SqliteConnectionStringBuilder(connectionString);
        sqlite.DataSource = Path.Combine(environment.ContentRootPath, sqlite.DataSource);

        services.AddDbContext<RoomHubDbContext>(options => options.UseSqlite(sqlite.ConnectionString));
        services.AddScoped<IUnitOfWork>(provider => provider.GetRequiredService<RoomHubDbContext>());
        services.AddScoped<IRoomRepository, RoomRepository>();
        services.AddScoped<IBookingRepository, BookingRepository>();

        return services;
    }

    public static WebApplication EnsureDatabaseCreated(this WebApplication app)
    {
        using var scope = app.Services.CreateScope();
        scope.ServiceProvider.GetRequiredService<RoomHubDbContext>().Database.EnsureCreated();
        return app;
    }
}
