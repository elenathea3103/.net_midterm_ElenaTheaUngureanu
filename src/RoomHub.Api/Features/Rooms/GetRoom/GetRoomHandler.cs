using RoomHub.Api.Common;
using RoomHub.Api.Domain.Rooms;

namespace RoomHub.Api.Features.Rooms.GetRoom;

public sealed class GetRoomHandler(IRoomRepository rooms)
    : IQueryHandler<GetRoomQuery, Result<GetRoomResponse>>
{
    public async Task<Result<GetRoomResponse>> Handle(GetRoomQuery query, CancellationToken cancellationToken)
    {
        var room = await rooms.GetByIdAsync(query.Id, cancellationToken);
        if (room is null)
            return RoomErrors.NotFound;

        return new GetRoomResponse(room.Id, room.Name, room.Capacity);
    }
}
