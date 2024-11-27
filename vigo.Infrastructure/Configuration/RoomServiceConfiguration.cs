using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using vigo.Domain.Entity;

namespace vigo.Infrastructure.Configuration
{
    internal class RoomServiceConfiguration : IEntityTypeConfiguration<RoomServiceR>
    {
        public void Configure(EntityTypeBuilder<RoomServiceR> builder)
        {
            builder.ToTable("roomService");
            builder.HasKey(e => e.Id);
            builder.Property(e => e.Id).ValueGeneratedOnAdd();
        }
    }
}
