using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EShop.Core.Constants;
using EShop.Core.Entities.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EShop.Data.DbConfigurations.Identity
{
    public class UserTokenDbConfiguration : IEntityTypeConfiguration<UserToken>
    {
        public void Configure(EntityTypeBuilder<UserToken> builder)
        {
            // Composite key
            builder.HasKey(p => new { p.UserId, p.LoginProvider, p.Name });
            builder.Property(p => p.LoginProvider).HasMaxLength(128);
            builder.Property(p => p.Name).HasMaxLength(128);
            builder.ToTable(TableNames.UserTokens);
        }
    }
}
