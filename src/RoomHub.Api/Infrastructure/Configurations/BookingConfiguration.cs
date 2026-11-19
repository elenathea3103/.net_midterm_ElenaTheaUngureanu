using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RoomHub.Api.Domain.Bookings;
using RoomHub.Api.Domain.Rooms;

namespace RoomHub.Api.Infrastructure.Configurations;

public sealed class BookingConfiguration : IEntityTypeConfiguration<Booking>
{
    public const int StatusMaxLength = 20;

    public void Configure(EntityTypeBuilder<Booking> builder)
    {
        builder.ToTable("Bookings");
        builder.HasKey(booking => booking.Id);
        builder.Property(booking => booking.Id).ValueGeneratedNever();
        builder.Property(booking => booking.BookedBy).IsRequired().HasMaxLength(Booking.BookedByMaxLength);
        builder.Property(booking => booking.StartUtc).IsRequired().HasConversion<UtcDateTimeConverter>();
        builder.Property(booking => booking.EndUtc).IsRequired().HasConversion<UtcDateTimeConverter>();
        builder.Property(booking => booking.Attendees).IsRequired();
        builder.Property(booking => booking.Status).IsRequired().HasConversion<string>().HasMaxLength(StatusMaxLength);

        builder.HasOne<Room>()
            .WithMany()
            .HasForeignKey(booking => booking.RoomId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasIndex(booking => new { booking.RoomId, booking.StartUtc });
    }
}
