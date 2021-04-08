using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EShop.Core.Entities.Identity
{
    public class Permission : BaseModel<int>
    {
        public string PermissionName { get; set; }
        public string Description { get; set; }
        public ICollection<RolePermission> RolePermissions { get; set; }
    }
}
