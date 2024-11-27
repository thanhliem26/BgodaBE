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
    internal class SystemEmployeeConfiguration : IEntityTypeConfiguration<SystemEmployee>
    {
        public void Configure(EntityTypeBuilder<SystemEmployee> builder)
        {
            builder.ToTable("systemEmployee");
            builder.HasKey(e => e.Id);
            builder.Property(e => e.Id).ValueGeneratedOnAdd();
        }
    }
}
