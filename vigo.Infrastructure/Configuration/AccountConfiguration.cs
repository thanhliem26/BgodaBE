using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using vigo.Domain.AccountFolder;
using vigo.Domain.User;

namespace vigo.Infrastructure.Configuration
{
    internal class AccountConfiguration : IEntityTypeConfiguration<Account>
    {
        public void Configure(EntityTypeBuilder<Account> builder)
        {
            builder.ToTable("account");
            builder.HasKey(e => e.Id);
            builder.Property(c => c.CreatedDate).HasColumnType("TIMESTAMP");
            builder.Property(c => c.UpdatedDate).HasColumnType("TIMESTAMP");
            builder.Property(c => c.DeletedDate).HasColumnType("TIMESTAMP");
            builder.HasOne<Role>().WithMany().HasForeignKey(e => e.RoleId).OnDelete(DeleteBehavior.NoAction);
            builder.HasOne<SystemEmployee>().WithOne().HasForeignKey<SystemEmployee>(e => e.AccountId).OnDelete(DeleteBehavior.NoAction);
            builder.HasOne<Tourist>().WithOne().HasForeignKey<Tourist>(e => e.AccountId).OnDelete(DeleteBehavior.NoAction);
            builder.HasOne<BusinessPartner>().WithOne().HasForeignKey<BusinessPartner>(e => e.AccountId).OnDelete(DeleteBehavior.NoAction);
        }
    }
}
