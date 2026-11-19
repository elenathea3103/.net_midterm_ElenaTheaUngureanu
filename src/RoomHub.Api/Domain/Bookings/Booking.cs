using System.Linq.Expressions;
using RoomHub.Api.Common;
using RoomHub.Api.Domain.Rooms;

namespace RoomHub.Api.Domain.Bookings;

public sealed class Booking
{
    public const int BookedByMaxLength = 200;

    public static readonly TimeSpan MaxDuration = TimeSpan.FromHours(4);

    private Booking()
    {
    }

    private Booking(Guid id, Guid roomId, string bookedBy, DateTime startUtc, DateTime endUtc, int attendees)
    {
        Id = id;
        RoomId = roomId;
        BookedBy = bookedBy;
        StartUtc = startUtc;
        EndUtc = endUtc;
        Attendees = attendees;
        Status = BookingStatus.Active;
    }

    public Guid Id { get; private set; }

    public Guid RoomId { get; private set; }

    public string BookedBy { get; private set; } = string.Empty;

    public DateTime StartUtc { get; private set; }

    public DateTime EndUtc { get; private set; }

    public int Attendees { get; private set; }

    public BookingStatus Status { get; private set; }

    public static Result<Booking> Create(
        Room room,
        string? bookedBy,
        DateTime startUtc,
        DateTime endUtc,
        int attendees,
        DateTime nowUtc)
    {
        startUtc = AsUtc(startUtc);
        endUtc = AsUtc(endUtc);

        if (string.IsNullOrWhiteSpace(bookedBy))
            return BookingErrors.BookedByRequired;

        if (endUtc <= startUtc)
            return BookingErrors.InvalidRange;

        if (attendees <= 0)
            return BookingErrors.AttendeesNotPositive;

        if (!room.CanHost(attendees))
            return BookingErrors.AttendeesExceedCapacity;

        if (startUtc < AsUtc(nowUtc))
            return BookingErrors.StartsInPast;

        if (endUtc - startUtc > MaxDuration)
            return BookingErrors.TooLong;

        return new Booking(Guid.NewGuid(), room.Id, bookedBy.Trim(), startUtc, endUtc, attendees);
    }

    public static Expression<Func<Booking, bool>> IsActiveAndOverlaps(Guid roomId, DateTime startUtc, DateTime endUtc) =>
        booking => booking.RoomId == roomId
                   && booking.Status == BookingStatus.Active
                   && booking.StartUtc < endUtc
                   && startUtc < booking.EndUtc;

    public Result Cancel()
    {
        if (Status == BookingStatus.Cancelled)
            return BookingErrors.AlreadyCancelled;

        Status = BookingStatus.Cancelled;
        return Result.Success();
    }

    private static DateTime AsUtc(DateTime value) => value.Kind switch
    {
        DateTimeKind.Utc => value,
        DateTimeKind.Local => value.ToUniversalTime(),
        _ => DateTime.SpecifyKind(value, DateTimeKind.Utc)
    };
}
