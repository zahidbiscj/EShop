using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace EShop.Core.Helpers
{
    public interface ICurrentUser
    {
        Guid UserId { get; }
        string Role { get; }
        void SetClaims(IEnumerable<Claim> claims);
    }
}
