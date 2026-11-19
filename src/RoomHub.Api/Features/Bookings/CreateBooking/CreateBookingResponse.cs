namespace RoomHub.Api.Features.Bookings.CreateBooking;

public sealed record CreateBookingResponse(Guid Id, Guid RoomId, string Status);
