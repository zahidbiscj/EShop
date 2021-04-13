using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using EShop.Core.Entities.Identity;
using EShop.Core.Helpers;

namespace EShop.Data.MappingProfiles
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<User, SeedUsersModel>().ReverseMap();
            CreateMap<Role, SeedRolesModel>().ReverseMap();
        }
    }
}
