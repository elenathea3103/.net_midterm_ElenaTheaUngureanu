using RoomHub.Api.Domain.Rooms;

namespace RoomHub.Api.Repositories;

public sealed class RoomRepository : IRoomRepository
{
    private readonly List<Room> _rooms = new();
    private readonly Lock _lock = new();

    public Task<Room?> GetByIdAsync(Guid id, CancellationToken cancellationToken)
    {
        lock (_lock)
        {
            return Task.FromResult(_rooms.FirstOrDefault(room => room.Id == id));
        }
    }

    public void Add(Room room)
    {
        lock (_lock)
        {
            _rooms.Add(room);
        }
    }
}
