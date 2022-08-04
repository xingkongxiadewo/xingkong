using Auth.DTO.Entity.System;
using Auth.DTO.SeacrhModel.Role;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Auth.Service.IService.System
{
    public interface IRoleService
    {
        /// <summary>
        /// 获取角色分页列表
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        Task<(int, List<Role>)> GetRolePageList(RoleSearchModel model);

        /// <summary>
        /// 新增角色
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="userName"></param>
        /// <param name="model"></param>
        /// <returns></returns>
        Task<(bool, string)> AddRole(int userId, string userName, AddRoleModel model);

        /// <summary>
        /// 修改角色
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="userName"></param>
        /// <param name="model"></param>
        /// <returns></returns>
        Task<(bool, string)> EditRole(int userId, string userName, EditRoleModel model);

        /// <summary>
        /// 删除角色
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<(bool, string)> DeleteRole(int id);

        /// <summary>
        /// 获取角色列表
        /// </summary>
        Task<List<Role>> GetRoleList();

        /// <summary>
        /// 修改状态
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<(bool, string)> EditStatus(int userId, string userName, int id);
    }
}
