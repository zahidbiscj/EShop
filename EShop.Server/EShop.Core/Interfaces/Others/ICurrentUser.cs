using System;
using System.Collections.Generic;
using System.Security.Claims;

namespace EShop.Core.Interfaces.Others
{
    public interface ICurrentUser
    {
        int UserId { get; }
        string Role { get; }
        void SetClaims(IEnumerable<Claim> claims);
    }
}
