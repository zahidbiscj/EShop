using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using EShop.Core.Dto.ResponseModels;
using EShop.Core.Entities.Identity;

namespace EShop.Data.MappingProfiles
{
    public class PermissionMappingProfile : Profile
    {
        public PermissionMappingProfile()
        {
            CreateMap<Permission, PermissionResponseModel>().ReverseMap();
        }
    }
}
