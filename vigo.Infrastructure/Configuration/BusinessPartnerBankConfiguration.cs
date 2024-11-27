using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using vigo.Domain.AccountFolder;
using vigo.Domain.Entity;

namespace vigo.Infrastructure.Configuration
{
    internal class BusinessPartnerBankConfiguration : IEntityTypeConfiguration<BusinessPartnerBank>
    {
        public void Configure(EntityTypeBuilder<BusinessPartnerBank> builder)
        {
            builder.ToTable("businessPartnerBank");
            builder.HasKey(e => e.Id);
            builder.Property(e => e.Id).ValueGeneratedOnAdd();
            builder.Property(c => c.CreatedDate).HasColumnType("TIMESTAMP");
            builder.Property(c => c.UpdatedDate).HasColumnType("TIMESTAMP");
            builder.Property(c => c.DeletedDate).HasColumnType("TIMESTAMP");
        }
    }
}
