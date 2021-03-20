using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EShop.Core.Constants;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EShop.Data.DbConfigurations.Identity
{
    public class UserTokenDbConfiguration : IEntityTypeConfiguration<IdentityUserToken<Guid>>
    {
        public void Configure(EntityTypeBuilder<IdentityUserToken<Guid>> builder)
        {
            // Composite key
            builder.HasKey(p => new { p.UserId, p.LoginProvider, p.Name });
            builder.Property(p => p.LoginProvider).HasMaxLength(128);
            builder.Property(p => p.Name).HasMaxLength(128);
            builder.ToTable(TableNames.UserTokens);
        }
    }
}
