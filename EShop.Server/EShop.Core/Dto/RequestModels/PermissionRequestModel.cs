using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EShop.Core.Dto.RequestModels
{
    public class PermissionRequestModel
    {
        public int Id { get; set; }
      //  [Required]
        public string PermissionName { get; set; }
        public string Description { get; set; }
    }
}
