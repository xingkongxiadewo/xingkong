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
    public class UserRoleService : IUserRoleService
    {

        private readonly AuthDbContext _db;
        public UserRoleService(AuthDbContext db)
        {
            _db = db;
        }

        public async Task<List<Role>> GetRolesByUserId(int userId)
        {
            return await _db.UserRoles.AsNoTracking().Where(x => x.UserId == userId)
                .Select(x => x.Role).Where(x => x.IsEnable)
                .ToListAsync();
        }

        /// <summary>
        /// 根据用户Id 获取用户绑定的角色
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public async Task<List<UserRole>> GetUserRoles(int userId)
        {
            return await _db.UserRoles.AsNoTracking().Where(x => x.UserId == userId).ToListAsync();
        }

        /// <summary>
        /// 绑定角色
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public async Task<bool> BindRole(BindRoleModel model)
        {
            var userRoles = await _db.UserRoles.Where(x => x.UserId == model.UserId).ToListAsync();

            var removeRoles = userRoles.Where(x => !model.RoleIds.Contains(x.RId)).ToList();

            var newRoleIds = model.RoleIds.Where(x => !userRoles.Any(y => y.RId == x)).ToList();

            _db.UserRoles.RemoveRange(removeRoles);
            await _db.UserRoles.AddRangeAsync(newRoleIds.Select(x => new UserRole
            {
                RId = x,
                UserId = model.UserId
            }));
            await _db.SaveChangesAsync();

            return true;
        }
    }
}
