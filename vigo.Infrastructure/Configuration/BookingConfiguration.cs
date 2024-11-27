using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using vigo.Domain.Entity;
using vigo.Domain.User;

namespace vigo.Infrastructure.Configuration
{
    internal class BookingConfiguration : IEntityTypeConfiguration<Booking>
    {
        public void Configure(EntityTypeBuilder<Booking> builder)
        {
            builder.ToTable("booking");
            builder.HasKey(e => e.Id);
            builder.Property(e => e.Id).ValueGeneratedOnAdd();
            builder.Property(c => c.CreatedDate).HasColumnType("TIMESTAMP");
            builder.Property(c => c.CheckInDate).HasColumnType("TIMESTAMP");
            builder.Property(c => c.CheckOutDate).HasColumnType("TIMESTAMP");
            builder.HasOne<Tourist>().WithMany().HasForeignKey(e => e.TouristId).OnDelete(DeleteBehavior.NoAction);
            builder.HasOne<Room>().WithMany().HasForeignKey(e => e.RoomId).OnDelete(DeleteBehavior.NoAction);
        }
    }
}
