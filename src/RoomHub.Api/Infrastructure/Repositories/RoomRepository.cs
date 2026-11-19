using Microsoft.EntityFrameworkCore;
using RoomHub.Api.Domain.Rooms;

namespace RoomHub.Api.Infrastructure.Repositories;

public sealed class RoomRepository(RoomHubDbContext context) : IRoomRepository
{
    public Task<Room?> GetByIdAsync(Guid id, CancellationToken cancellationToken) =>
        context.Rooms.FirstOrDefaultAsync(room => room.Id == id, cancellationToken);

    public void Add(Room room) => context.Rooms.Add(room);
}
