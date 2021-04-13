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
    public class UserLoginDbConfiguration :  IEntityTypeConfiguration<UserLogin>
    {
        public void Configure(EntityTypeBuilder<UserLogin> builder)
        {
            builder.HasKey(x => x.UserId);
            builder.Property(p => p.ProviderKey).HasMaxLength(128);
            builder.Property(p => p.LoginProvider).HasMaxLength(128);
            builder.ToTable(TableNames.UserLogins);
        }
    }
}
