using Auth.DTO.ApiResponseModel;
using Auth.DTO.ApiResponseModel.Menu;
using Auth.DTO.Entity.System;
using Auth.DTO.SeacrhModel.Menu;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Auth.Service.IService.System
{
    public interface IMenuService
    {
        /// <summary>
        /// 根据角色Id列表 获取菜单列表
        /// </summary>
        /// <param name="roleIds"></param>
        /// <returns></returns>
        Task<List<Menu>> GetMenuByRoleId(List<int> roleIds);

        /// <summary>
        /// 获取菜单分页
        /// </summary>
        /// <param name="menu"></param>
        /// <returns></returns>
        Task<(int, List<Menu>)> GetMenuPageList(MenuSearchModel model);

        /// <summary>
        /// 新增菜单
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        Task<(bool, string)> AddMenu(int userId, string userName, AddMenuModel model);

        /// <summary>
        /// 修改菜单
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        Task<(bool, string)> EditMenu(int userId, string userName, EditMenuModel model);

        /// <summary>
        /// 删除菜单
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<(bool, string)> DeleteMenu(int id);

        /// <summary>
        /// 获取菜单下拉列表
        /// </summary>
        /// <returns></returns>
        Task<List<SelectModel>> GetMenuSelect();

        /// <summary>
        /// 获取菜单列表
        /// </summary>
        /// <returns></returns>
        Task<List<Menu>> GetMenuList();

        /// <summary>
        /// 绑定菜单
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        Task<bool> BindMenu(BindMenuModel model);
        /// <summary>
        /// 修改状态
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="userName"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<(bool, string)> EditStatus(int userId, string userName, int id);
    }
}
