using Auth.Common;
using Auth.DTO.ApiResponseModel.User;
using Auth.DTO.Common;
using Auth.DTO.Entity.System;
using Auth.DTO.SeacrhModel.User;
using Auth.DTO.SearchModel.User;
using Auth.Service.IService.System;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Auth.Service.Service.System
{
    public class UserService : IUserService
    {
        private readonly AuthDbContext _db;
        public UserService(AuthDbContext db)
        {
            _db = db;
        }

        /// <summary>
        /// 登入
        /// </summary>
        /// <param name="account"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        public async Task<User> GetUserByLogin(string account, string password)
        {
            var pwd = UserEncodeUtil.EncodePassword(password);
            return await _db.Users.AsNoTracking().FirstOrDefaultAsync(x => x.IsEnable && x.Account == account && x.Password == pwd);
        }

        /// <summary>
        /// 根据Account 获取用户信息
        /// </summary>
        /// <param name="account"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        public async Task<User> GetUserByAccount(string account)
        {
            return await _db.Users.AsNoTracking().FirstOrDefaultAsync(x => x.IsEnable && x.Account == account);
        }

        /// <summary>
        /// 根据Id 获取User信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<User> GetUserById(int id)
        {
            return await _db.Users.AsNoTracking().FirstOrDefaultAsync(x => x.IsEnable && x.Id == id);
        }

        /// <summary>
        /// 获取用户列表分页
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public async Task<(int, List<User>)> GetUserPageList(UserSearchModel model)
        {
            var query = _db.Users.AsNoTracking();

            if (!string.IsNullOrEmpty(model.Name))
                query = query.Where(x => x.Name.Contains(model.Name));

            if (!string.IsNullOrEmpty(model.Phone))
                query = query.Where(x => x.Phone.Contains(model.Phone));

            if (!string.IsNullOrEmpty(model.Account))
                query = query.Where(x => x.Account.Contains(model.Account));

            var total = await query.CountAsync();
            var data = await query.OrderBy(x => x.Id).Skip(model.Skip).Take(model.Take).ToListAsync();
            return (total, data);
        }

        /// <summary>
        /// 新增用户
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="userName"></param>
        /// <param name="model"></param>
        /// <returns></returns>
        public async Task<(bool, string)> AddUser(int userId, string userName, AddUserModel model)
        {
            if (await _db.Users.AsNoTracking().AnyAsync(x => x.Account == model.Account))
                return (false, "当前用户已存在!");

            await _db.Users.AddAsync(new User
            {
                Account = model.Account,
                CreateTime = DateTime.Now,
                CreateUserId = userId,
                CreateUserName = userName,
                Email = model.Email,
                ModifieyTime = DateTime.Now,
                ModifieyUserId = userId,
                ModifieyUserName = userName,
                Name = model.Name,
                Password = UserEncodeUtil.EncodePassword(model.Password),
                Phone = model.Phone,
                IsEnable = true
            });
            await _db.SaveChangesAsync();
            return (true, "新增成功");
        }

        /// <summary>
        /// 修改用户
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="userName"></param>
        /// <param name="model"></param>
        /// <returns></returns>
        public async Task<(bool, string)> EditUser(int userId, string userName, EditUserModel model)
        {
            var user = await _db.Users.FirstOrDefaultAsync(x => x.Id == model.Id);
            if (user is null)
                return (false, "当前用户不存在!");

            if (await _db.Users.AsNoTracking().AnyAsync(x => x.Account == model.Account && x.Id != model.Id))
                return (false, "当前用户已存在!");

            user.Name = model.Name;
            user.Account = model.Account;
            user.Phone = model.Phone;
            user.Email = model.Email;
            user.ModifieyTime = DateTime.Now;
            user.ModifieyUserId = userId;
            user.ModifieyUserName = userName;

            await _db.SaveChangesAsync();
            return (true, "修改成功");
        }

        /// <summary>
        /// 删除用户
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<bool> DeleteUser(int id)
        {
            var user = await _db.Users.FirstOrDefaultAsync(x => x.Id == id);
            _db.Users.Remove(user);
            await _db.SaveChangesAsync();
            return true;
        }

        /// <summary>
        /// 修改密码
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="userName"></param>
        /// <param name="model"></param>
        /// <returns></returns>
        public async Task<(bool, string)> EditPassword(int userId, string userName, EditPasswordModel model)
        {
            var user = await _db.Users.FirstOrDefaultAsync(x => x.Id == userId);
            if (user is null)
                return (false, "当前用户无效,请刷新后重试!");

            var oldPassword = UserEncodeUtil.EncodePassword(model.OldPassword);

            if (oldPassword != user.Password)
                return (false, "密码错误,请输入正确的密码!");

            user.Password = UserEncodeUtil.EncodePassword(model.NewPassword);
            user.ModifieyTime = DateTime.Now;
            user.ModifieyUserId = userId;
            user.ModifieyUserName = userName;
            await _db.SaveChangesAsync();
            return (true, "修改成功!");
        }

        /// <summary>
        /// 修改状态
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<(bool, string)> EditStatus(int userId, string userName, int id)
        {
            var user = await _db.Users.FirstOrDefaultAsync(x => x.Id == id);
            if (user is null)
                return (false, "当前用户无效,请刷新后重试!");
            user.ModifieyTime = DateTime.Now;
            user.ModifieyUserId = userId;
            user.ModifieyUserName = userName;
            user.IsEnable = !user.IsEnable;
            await _db.SaveChangesAsync();
            return (true, "修改状态成功!");
        }
    }
}
