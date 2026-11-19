using RoomHub.Api.Domain.Bookings;

namespace RoomHub.Api.Repositories;

public sealed class BookingRepository : IBookingRepository
{
    private readonly List<Booking> _bookings = new();
    private readonly Lock _lock = new();

    public Task<Booking?> GetByIdAsync(Guid id, CancellationToken cancellationToken)
    {
        lock (_lock)
        {
            return Task.FromResult(_bookings.FirstOrDefault(booking => booking.Id == id));
        }
    }

    public Task<bool> HasActiveOverlapAsync(Guid roomId, DateTime startUtc, DateTime endUtc, CancellationToken cancellationToken)
    {
        lock (_lock)
        {
            var overlaps = Booking.IsActiveAndOverlaps(roomId, startUtc, endUtc);
            return Task.FromResult(_bookings.AsQueryable().Any(overlaps));
        }
    }

    public void Add(Booking booking)
    {
        lock (_lock)
        {
            _bookings.Add(booking);
        }
    }
}
