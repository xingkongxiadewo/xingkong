using Auth.DTO.ApiResponseModel.Menu;
using Auth.DTO.ApiResponseModel.Role;
using Auth.DTO.ApiResponseModel.User;
using Auth.DTO.Entity.System;
using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Auth.DTO
{
    public class AutoMapperConfig : Profile
    {
        public AutoMapperConfig()
        {
            CreateMap<User, UserModel>();
            CreateMap<Role, RoleModel>();
            CreateMap<Menu, MenuModel>();
        }
    }
}
