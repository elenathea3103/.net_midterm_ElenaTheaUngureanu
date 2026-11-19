using Microsoft.EntityFrameworkCore;
using RoomHub.Api.Domain.Bookings;

namespace RoomHub.Api.Infrastructure.Repositories;

public sealed class BookingRepository(RoomHubDbContext context) : IBookingRepository
{
    public Task<Booking?> GetByIdAsync(Guid id, CancellationToken cancellationToken) =>
        context.Bookings.FirstOrDefaultAsync(booking => booking.Id == id, cancellationToken);

    public Task<bool> HasActiveOverlapAsync(Guid roomId, DateTime startUtc, DateTime endUtc, CancellationToken cancellationToken) =>
        context.Bookings.AnyAsync(Booking.IsActiveAndOverlaps(roomId, startUtc, endUtc), cancellationToken);

    public void Add(Booking booking) => context.Bookings.Add(booking);
}
