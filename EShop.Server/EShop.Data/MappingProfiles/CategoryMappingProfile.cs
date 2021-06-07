using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using EShop.Core.Dto;
using EShop.Core.Dto.RequestModels;
using EShop.Core.Entities.Core;

namespace EShop.Data.MappingProfiles
{
    public class CategoryMappingProfile: Profile
    {
        public CategoryMappingProfile()
        {
            CreateMap<CategoryRequestModel, Category>().ReverseMap();
            CreateMap<CategoryModel, Category>().ReverseMap();
        }
    }
}
