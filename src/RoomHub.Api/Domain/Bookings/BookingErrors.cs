using RoomHub.Api.Common;

namespace RoomHub.Api.Domain.Bookings;

public static class BookingErrors
{
    public static readonly Error BookedByRequired =
        Error.Validation("InvalidBookedBy", "BookedBy is required.");

    public static readonly Error InvalidRange =
        Error.Validation("InvalidRange", "The end must be after the start.");

    public static readonly Error AttendeesNotPositive =
        Error.Validation("InvalidAttendees", "Attendees must be positive.");

    public static readonly Error AttendeesExceedCapacity =
        Error.Validation("AttendeesExceedCapacity", "The room does not have enough seats.");

    public static readonly Error StartsInPast =
        Error.Validation("StartInPast", "A booking cannot start in the past.");

    public static readonly Error TooLong =
        Error.Validation("BookingTooLong", "A booking cannot last more than 4 hours.");

    public static readonly Error TooLongForLargeRoom =
        Error.Validation("BookingTooLongForLargeRoom", "A room with more than 10 seats can be booked for at most 2 hours.");

    public static readonly Error CancellationTooLate =
        Error.Conflict("CancellationTooLate", "A booking can be cancelled only at least 60 minutes before it starts.");

    public static readonly Error RoomOccupied =
        Error.Conflict("RoomOccupied", "The room is occupied in this interval.");

    public static readonly Error DailyLimitReached =
        Error.Conflict("DailyBookingLimitReached", "A participant cannot have more than 2 active bookings on the same day.");

    public static readonly Error AlreadyCancelled =
        Error.Conflict("BookingAlreadyCancelled", "The booking is already cancelled.");

    public static readonly Error NotFound =
        Error.NotFound("BookingNotFound", "The booking does not exist.");
}
