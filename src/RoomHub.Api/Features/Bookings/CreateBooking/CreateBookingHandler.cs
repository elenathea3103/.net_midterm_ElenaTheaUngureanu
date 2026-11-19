using RoomHub.Api.Common;
using RoomHub.Api.Domain;
using RoomHub.Api.Domain.Bookings;
using RoomHub.Api.Domain.Rooms;

namespace RoomHub.Api.Features.Bookings.CreateBooking;

public sealed class CreateBookingHandler(
    IRoomRepository rooms,
    IBookingRepository bookings,
    IUnitOfWork unitOfWork,
    TimeProvider timeProvider)
    : ICommandHandler<CreateBookingCommand, Result<CreateBookingResponse>>
{
    public async Task<Result<CreateBookingResponse>> Handle(CreateBookingCommand command, CancellationToken cancellationToken)
    {
        var room = await rooms.GetByIdAsync(command.RoomId, cancellationToken);
        if (room is null)
            return RoomErrors.NotFound;

        var created = Booking.Create(
            room,
            command.BookedBy,
            command.StartUtc,
            command.EndUtc,
            command.Attendees,
            timeProvider.GetUtcNow().UtcDateTime);
        if (created.IsFailure)
            return created.Error;

        var booking = created.Value;
        if (await bookings.HasActiveOverlapAsync(booking.RoomId, booking.StartUtc, booking.EndUtc, cancellationToken))
            return BookingErrors.RoomOccupied;

        var activeOnSameDay = await bookings.CountActiveForParticipantOnDayAsync(
            booking.BookedBy, booking.StartUtc, cancellationToken);
        if (Booking.ReachesDailyLimit(activeOnSameDay))
            return BookingErrors.DailyLimitReached;

        bookings.Add(booking);
        await unitOfWork.SaveChangesAsync(cancellationToken);

        return new CreateBookingResponse(booking.Id, booking.RoomId, booking.Status.ToString());
    }
}
