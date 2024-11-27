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
    internal class EmailAuthenConfiguration : IEntityTypeConfiguration<EmailAuthen>
    {
        public void Configure(EntityTypeBuilder<EmailAuthen> builder)
        {
            builder.ToTable("emailAuthen");
            builder.HasKey(e => e.Id);
            builder.Property(e => e.Id).ValueGeneratedOnAdd();
            builder.HasOne<Account>().WithMany().HasForeignKey(e => e.AccountId).OnDelete(DeleteBehavior.NoAction);
        }
    }
}
