using Auth.DTO.Entity.System;
using Auth.DTO.SeacrhModel.Role;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Auth.Service.IService.System
{
    public interface IUserRoleService
    {
        /// <summary>
        /// 根据用户Id 获取角色列表
        /// </summary>
        /// <param name="UserId"></param>
        /// <returns></returns>
        Task<List<Role>> GetRolesByUserId(int userId);

        /// <summary>
        /// 根据用户Id 获取用户绑定的角色
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        Task<List<UserRole>> GetUserRoles(int userId);

        /// <summary>
        /// 绑定角色
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        Task<bool> BindRole(BindRoleModel model);

    }
}
