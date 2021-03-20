using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace EShop.Core.Entities.Identity
{
    public class User : IdentityUser<Guid>
    {
        public string FullName { get; set; }
        public bool IsDeleted { get; set; }
        public ICollection<UserRole> UserRoles { get; set; }
    }
}
