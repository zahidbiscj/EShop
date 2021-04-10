using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using EShop.Core.Interfaces.Others;

namespace EShop.Core.Helpers
{
    public class CurrentUserService : ICurrentUser
    {
        private List<Claim> _claims;
        public int UserId
        {
            get
            {
                var userId = Convert.ToInt32(GetClaim(ClaimTypes.NameIdentifier));
                return userId;
            }
        }

        public string Role
        {
            get
            {
                var role = GetClaim(ClaimTypes.Role);
                return role.ToLower();
            }
        }

        public void SetClaims(IEnumerable<Claim> claims)
        {
            _claims = claims.ToList();
        }

        public string FullName
        {
            get
            {
                var name = GetClaim(ClaimTypes.Name);
                return name;
            }
        }

        private string GetClaim(string type)
        {
            return _claims.SingleOrDefault(c => c.Type == type)?.Value;
        }
    }
}
