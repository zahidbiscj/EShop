using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using EShop.Core.Dto.RequestModels;
using EShop.Core.Entities.Identity;

namespace EShop.Data.MappingProfiles
{
    public class AuthMappingProfile :  Profile
    {
        public AuthMappingProfile()
        {
            CreateMap<User, RegisterRequestModel>().ReverseMap();
        }
    }
}
