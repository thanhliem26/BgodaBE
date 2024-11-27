using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using vigo.Domain.AccountFolder;
using vigo.Domain.Entity;
using vigo.Domain.User;

namespace vigo.Infrastructure.Configuration
{
    internal class RoomConfiguration : IEntityTypeConfiguration<Room>
    {
        public void Configure(EntityTypeBuilder<Room> builder)
        {
            builder.ToTable("room");
            builder.HasKey(e => e.Id);
            builder.Property(e => e.Id).ValueGeneratedOnAdd();
            builder.Property(c => c.CreatedDate).HasColumnType("TIMESTAMP");
            builder.Property(c => c.UpdatedDate).HasColumnType("TIMESTAMP");
            builder.Property(c => c.DeletedDate).HasColumnType("TIMESTAMP");
            builder.HasMany<RoomServiceR>().WithOne().HasForeignKey(e => e.RoomId).OnDelete(DeleteBehavior.NoAction);
            builder.HasOne<RoomType>().WithMany().HasForeignKey(e => e.RoomTypeId).OnDelete(DeleteBehavior.NoAction);
            builder.HasOne<BusinessPartner>().WithMany().HasForeignKey(e => e.BusinessPartnerId).OnDelete(DeleteBehavior.NoAction);
        }
    }
}
