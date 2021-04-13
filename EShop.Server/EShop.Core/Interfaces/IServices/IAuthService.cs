using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EShop.Core.Entities.Identity;

namespace EShop.Core.Interfaces.IServices
{
    public interface IAuthService
    {
        Task<string> GenerateToken(User user);
    }
}
