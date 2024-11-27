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
    internal class RatingConfiguration : IEntityTypeConfiguration<Rating>
    {
        public void Configure(EntityTypeBuilder<Rating> builder)
        {
            builder.ToTable("rating");
            builder.HasKey(e => e.Id);
            builder.Property(e => e.Id).ValueGeneratedOnAdd();
            builder.Property(c => c.LastUpdatedDate).HasColumnType("TIMESTAMP");
            builder.Property(c => c.DeletedDate).HasColumnType("TIMESTAMP");
            builder.HasOne<Tourist>().WithMany().HasForeignKey(e => e.TouristId).OnDelete(DeleteBehavior.NoAction);
            builder.HasOne<Room>().WithMany().HasForeignKey(e => e.RoomId).OnDelete(DeleteBehavior.NoAction);
        }
    }
}
