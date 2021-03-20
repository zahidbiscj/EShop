using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EShop.Core.Entities;

namespace EShop.Data
{
    public class EShopDbContext : IdentityDbContext<AppUser,>
    {
    }
}
