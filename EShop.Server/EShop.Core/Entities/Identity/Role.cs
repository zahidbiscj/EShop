using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EShop.Core.Interfaces.Others;
using Microsoft.AspNetCore.Identity;

namespace EShop.Core.Entities.Identity
{
    public class Role : IdentityRole<Guid>
    {
        public virtual ICollection<RolePermission> RolePermissions { get; set; }
        public ICollection<UserRole> UserRoles { get; set; }
    }
}
