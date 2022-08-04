using Auth.DTO.ApiResponseModel.User;
using Auth.DTO.Entity.System;
using Auth.DTO.SeacrhModel.User;
using Auth.DTO.SearchModel.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Auth.Service.IService.System
{
    public interface IUserService
    {
        /// <summary>
        /// 登入
        /// </summary>
        /// <param name="account"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        Task<User> GetUserByLogin(string account, string password);

        /// <summary>
        /// 根据Account 获取用户信息
        /// </summary>
        /// <param name="account"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        Task<User> GetUserByAccount(string account);

        /// <summary>
        /// 根据Id 获取User信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<User> GetUserById(int id);


        /// <summary>
        /// 获取用户列表分页
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        Task<(int, List<User>)> GetUserPageList(UserSearchModel model);

        /// <summary>
        /// 新增用户
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="userName"></param>
        /// <param name="model"></param>
        /// <returns></returns>
        Task<(bool, string)> AddUser(int userId, string userName, AddUserModel model);

        /// <summary>
        /// 修改用户
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="userName"></param>
        /// <param name="model"></param>
        /// <returns></returns>
        Task<(bool, string)> EditUser(int userId, string userName, EditUserModel model);

        /// <summary>
        /// 删除用户
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<bool> DeleteUser(int id);

        /// <summary>
        /// 修改密码
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="userName"></param>
        /// <param name="model"></param>
        /// <returns></returns>
        Task<(bool, string)> EditPassword(int userId, string userName, EditPasswordModel model);

        /// <summary>
        /// 修改状态
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<(bool, string)> EditStatus(int userId, string userName, int id);
    }
}
