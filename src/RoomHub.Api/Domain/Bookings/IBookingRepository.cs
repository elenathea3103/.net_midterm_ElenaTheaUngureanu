namespace RoomHub.Api.Domain.Bookings;

public interface IBookingRepository
{
    Task<Booking?> GetByIdAsync(Guid id, CancellationToken cancellationToken);

    Task<bool> HasActiveOverlapAsync(Guid roomId, DateTime startUtc, DateTime endUtc, CancellationToken cancellationToken);

    void Add(Booking booking);
}
