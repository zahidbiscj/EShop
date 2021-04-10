﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EShop.Core.Interfaces.IServices
{
    public interface IPermissionService
    {
        Task<List<string>> GetPermissionsByUserId(int userId);
    }
}
