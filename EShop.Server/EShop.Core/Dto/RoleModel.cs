using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EShop.Core.Entities.Identity;

namespace EShop.Core.Dto
{
    public class RoleModel
    {
        public RoleModel()
        {
            RolePermissions = new List<RolePermission>();
        }
        public int Id { get; set; }
        public string Name { get; set; }
        public ICollection<RolePermission> RolePermissions { get; set; }

    }
}
