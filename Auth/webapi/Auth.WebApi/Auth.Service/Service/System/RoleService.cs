using Auth.DTO.Entity.System;
using Auth.DTO.SeacrhModel.Role;
using Auth.Service.IService.System;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Auth.Service.Service.System
{
    public class RoleService : IRoleService
    {
        private readonly AuthDbContext _db;
        public RoleService(AuthDbContext db)
        {
            _db = db;
        }

        /// <summary>
        /// 新增角色
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="userName"></param>
        /// <param name="model"></param>
        /// <returns></returns>
        public async Task<(bool, string)> AddRole(int userId, string userName, AddRoleModel model)
        {
            if (await _db.Roles.AnyAsync(x => x.Name == model.Name))
                return (false, $"角色名称[{model.Name}]已存在!");
            await _db.Roles.AddAsync(new Role
            {
                CreateTime = DateTime.Now,
                CreateUserId = userId,
                CreateUserName = userName,
                IsEnable = true,
                ModifieyTime = DateTime.Now,
                ModifieyUserId = userId,
                ModifieyUserName = userName,
                Name = model.Name
            });

            await _db.SaveChangesAsync();
            return (true, "新增成功!");
        }

        /// <summary>
        /// 修改角色
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="userName"></param>
        /// <param name="model"></param>
        /// <returns></returns>
        public async Task<(bool, string)> EditRole(int userId, string userName, EditRoleModel model)
        {
            var role = await _db.Roles.FirstOrDefaultAsync(x => x.Id == model.Id);
            if (role is null)
                return (false, $"角色无效,请刷新页面后重试!");

            if (await _db.Roles.AnyAsync(x => x.Name == model.Name && x.Id != model.Id))
                return (false, $"角色名称[{model.Name}]已存在!");

            role.Name = model.Name;
            role.ModifieyUserId = userId;
            role.ModifieyUserName = userName;
            role.ModifieyTime = DateTime.Now;

            await _db.SaveChangesAsync();
            return (true, "修改成功!");
        }

        /// <summary>
        /// 获取角色分页列表
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public async Task<(int, List<Role>)> GetRolePageList(RoleSearchModel model)
        {
            var query = _db.Roles.AsNoTracking();

            if (!string.IsNullOrEmpty(model.Name))
                query = query.Where(x => x.Name == model.Name);

            var total = await query.CountAsync();
            var data = await query.OrderBy(x => x.Id).Skip(model.Skip).Take(model.Take).ToListAsync();
            return (total, data);
        }

        /// <summary>
        /// 删除角色
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<(bool, string)> DeleteRole(int id)
        {
            var role = await _db.Roles.FirstOrDefaultAsync(x => x.Id == id);
            if (role is null)
                return (false, $"角色无效,请刷新页面后重试!");

            var userRoles = await _db.UserRoles.Where(x => x.RId == role.Id).ToListAsync();

            _db.Roles.Remove(role);
            _db.UserRoles.RemoveRange(userRoles);

            await _db.SaveChangesAsync();
            return (true, "删除成功!");
        }

        /// <summary>
        /// 获取角色列表
        /// </summary>
        /// <returns></returns>
        public async Task<List<Role>> GetRoleList()
        {
            return await _db.Roles.AsNoTracking().Where(x => x.IsEnable).ToListAsync();
        }

        /// <summary>
        /// 修改状态
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<(bool, string)> EditStatus(int userId, string userName, int id)
        {
            var role = await _db.Roles.FirstOrDefaultAsync(x => x.Id == id);
            if (role is null)
                return (false, $"角色无效,请刷新页面后重试!");
            role.IsEnable = !role.IsEnable;
            role.ModifieyUserId = userId;
            role.ModifieyUserName = userName;
            role.ModifieyTime = DateTime.Now;

            await _db.SaveChangesAsync();
            return (true, "修改成功!");
        }
    }
}
