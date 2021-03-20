using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EShop.Core.Constants;
using EShop.Core.Entities.Identity;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EShop.Data.DbConfigurations.Identity
{
    public class RoleDbConfiguration : IEntityTypeConfiguration<Role>
    {
        public void Configure(EntityTypeBuilder<Role> builder)
        {
            builder.ToTable(TableNames.Roles);
            builder.HasKey(x => x.Id);
        }
    }
}
