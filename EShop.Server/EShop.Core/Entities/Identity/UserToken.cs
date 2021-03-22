using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace EShop.Core.Entities.Identity
{
    public class UserToken : IdentityUserToken<Guid>
    {
    }
}
