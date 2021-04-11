using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EShop.Core.Dto.ResponseModels
{
    public class PermissionResponseModel
    {
        public int Id { get; set; }
        public string PermissionName { get; set; }
        public string Description { get; set; }
    }
}
