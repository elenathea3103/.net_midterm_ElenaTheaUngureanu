using RoomHub.Api.Common;
using RoomHub.Api.Features.Bookings.CancelBooking;
using RoomHub.Api.Features.Bookings.CreateBooking;
using RoomHub.Api.Features.Bookings.GetBooking;
using RoomHub.Api.Features.Rooms.CreateRoom;
using RoomHub.Api.Features.Rooms.GetRoom;

namespace RoomHub.Api.Features;

public static class FeatureExtensions
{
    public static IServiceCollection AddFeatures(this IServiceCollection services)
    {
        services.AddScoped<ICommandHandler<CreateRoomCommand, Result<CreateRoomResponse>>, CreateRoomHandler>();
        services.AddScoped<IQueryHandler<GetRoomQuery, Result<GetRoomResponse>>, GetRoomHandler>();
        services.AddScoped<ICommandHandler<CreateBookingCommand, Result<CreateBookingResponse>>, CreateBookingHandler>();
        services.AddScoped<IQueryHandler<GetBookingQuery, Result<GetBookingResponse>>, GetBookingHandler>();
        services.AddScoped<ICommandHandler<CancelBookingCommand, Result>, CancelBookingHandler>();

        return services;
    }

    public static IEndpointRouteBuilder MapFeatures(this IEndpointRouteBuilder app)
    {
        app.MapCreateRoom();
        app.MapGetRoom();
        app.MapCreateBooking();
        app.MapGetBooking();
        app.MapCancelBooking();

        return app;
    }
}
