using RoomHub.Api.Common;

namespace RoomHub.Api.Features.Bookings.CancelBooking;

public static class CancelBookingEndpoint
{
    public static IEndpointRouteBuilder MapCancelBooking(this IEndpointRouteBuilder app)
    {
        app.MapPost("/api/bookings/{id:guid}/cancel", async (
                Guid id,
                ICommandHandler<CancelBookingCommand, Result> handler,
                CancellationToken cancellationToken) =>
            {
                var result = await handler.Handle(new CancelBookingCommand(id), cancellationToken);

                return result.IsSuccess
                    ? Results.NoContent()
                    : result.Error.ToProblem();
            })
            .WithTags("Bookings");

        return app;
    }
}
