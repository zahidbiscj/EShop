using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EShop.Core.Interfaces.Others
{
    public interface IBaseEntity
    {
        DateTime CreatedAt { get; set; }
        int CreatedBy { get; set; }
        DateTime? LastUpdated { get; set; }
        int? UpdatedBy { get; set; }
        bool IsDeleted { get; set; }
    }
}
