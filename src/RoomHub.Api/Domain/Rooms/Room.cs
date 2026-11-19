using RoomHub.Api.Common;

namespace RoomHub.Api.Domain.Rooms;

public sealed class Room
{
    public const int NameMaxLength = 200;

    public const int LargeRoomCapacityThreshold = 10;

    public static readonly TimeSpan StandardMaxBookingDuration = TimeSpan.FromHours(4);

    public static readonly TimeSpan LargeRoomMaxBookingDuration = TimeSpan.FromHours(2);

    private Room()
    {
    }

    private Room(Guid id, string name, int capacity)
    {
        Id = id;
        Name = name;
        Capacity = capacity;
    }

    public Guid Id { get; private set; }

    public string Name { get; private set; } = string.Empty;

    public int Capacity { get; private set; }

    public static Result<Room> Create(string? name, int capacity)
    {
        if (string.IsNullOrWhiteSpace(name))
            return RoomErrors.NameRequired;

        if (capacity <= 0)
            return RoomErrors.CapacityNotPositive;

        return new Room(Guid.NewGuid(), name.Trim(), capacity);
    }

    public bool CanHost(int attendees) => attendees <= Capacity;

    public bool IsLarge => Capacity > LargeRoomCapacityThreshold;

    public TimeSpan MaxBookingDuration => IsLarge ? LargeRoomMaxBookingDuration : StandardMaxBookingDuration;
}
