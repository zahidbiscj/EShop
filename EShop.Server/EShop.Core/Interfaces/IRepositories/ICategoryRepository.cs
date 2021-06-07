using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EShop.Core.Entities.Core;

namespace EShop.Core.Interfaces.IRepositories
{
    public interface ICategoryRepository: IBaseRepository<Category,int>
    {
    }
}
