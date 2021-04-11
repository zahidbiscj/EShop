using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EShop.Core.Dto.RequestModels
{
    public class RoleRequestModel
    {
        public int? Id { get; set; }
        [Required]
        public string RoleName { get; set; }
        public List<PermissionRequestModel> Permissions { get; set; }
    }
}
