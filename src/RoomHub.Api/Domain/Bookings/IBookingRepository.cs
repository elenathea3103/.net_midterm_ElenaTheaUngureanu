namespace RoomHub.Api.Domain.Bookings;

public interface IBookingRepository
{
    Task<Booking?> GetByIdAsync(Guid id, CancellationToken cancellationToken);

    Task<bool> HasActiveOverlapAsync(Guid roomId, DateTime startUtc, DateTime endUtc, CancellationToken cancellationToken);

    Task<int> CountActiveForParticipantOnDayAsync(string bookedBy, DateTime dayUtc, CancellationToken cancellationToken);

    void Add(Booking booking);
}
