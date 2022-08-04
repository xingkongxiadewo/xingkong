using Auth.DTO.ApiResponseModel;
using Auth.DTO.ApiResponseModel.Menu;
using Auth.DTO.Entity.System;
using Auth.DTO.SeacrhModel.Menu;
using Auth.Service.IService.System;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Auth.Service.Service.System
{
    public class MenuService : IMenuService
    {
        private readonly AuthDbContext _db;
        private readonly IMapper _mapper;
        public MenuService(AuthDbContext db, IMapper mapper)
        {
            _db = db;
            _mapper = mapper;
        }

        public async Task<List<Menu>> GetMenuByRoleId(List<int> roleIds)
        {
            var menus = await _db.RoleMenus.AsNoTracking().Where(x => roleIds.Contains(x.RId))
                .Select(x => x.Menu).Where(x => x.IsEnable).Distinct()
                .ToListAsync();
            return menus;
        }

        /// <summary>
        /// 获取菜单分页
        /// </summary>
        /// <param name="menu"></param>
        /// <returns></returns>
        public async Task<(int, List<Menu>)> GetMenuPageList(MenuSearchModel model)
        {
            var query = _db.Menus.AsNoTracking();
            if (!string.IsNullOrEmpty(model.Name))
                query = query.Where(x => x.Name.Contains(model.Name));
            var total = await query.CountAsync();
            var result = await query.OrderBy(x => x.Id).Skip(model.Skip).Take(model.Take).ToListAsync();

            return (total, result);
        }

        /// <summary>
        /// 新增菜单
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public async Task<(bool, string)> AddMenu(int userId, string userName, AddMenuModel model)
        {
            if (await _db.Menus.AsNoTracking().AnyAsync(x => x.Name == model.Name && x.Url == model.Url))
                return (false, "当前菜单已存在!");

            var data = await _db.Menus.AsNoTracking().Where(x => x.ParentId == model.ParentId).Select(x => x.Sort).ToListAsync();

            var sort = data.Any() ? data.Max() : 0;

            await _db.Menus.AddAsync(new Menu
            {
                CreateTime = DateTime.Now,
                Name = model.Name,
                CreateUserId = userId,
                CreateUserName = userName,
                Icon = model.Icon ?? "",
                IsEnable = true,
                ModifieyTime = DateTime.Now,
                ModifieyUserId = userId,
                ModifieyUserName = userName,
                ParentId = model.ParentId ?? 0,
                Sort = sort,
                Url = model.Url
            });

            await _db.SaveChangesAsync();
            return (true, "新增成功");
        }

        /// <summary>
        /// 修改菜单
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public async Task<(bool, string)> EditMenu(int userId, string userName, EditMenuModel model)
        {
            if (await _db.Menus.AsNoTracking().AnyAsync(x => x.Name == model.Name && x.Url == model.Url && x.Id != model.Id))
                return (false, $"当前菜单{model.Name}已存在!");

            var data = await _db.Menus.FirstOrDefaultAsync(x => x.Id == model.Id);

            if (data is null)
                return (false, $"菜单不存在,请刷新页面后重试!");


            data.ModifieyUserName = userName;
            data.Icon = model.Icon ?? "";
            data.ModifieyUserId = userId;
            data.ModifieyTime = DateTime.Now;
            data.Url = model.Url;

            var parentId = model.ParentId ?? 0;
            if (data.ParentId != model.ParentId)
            {
                var menus = await _db.Menus.AsNoTracking().Where(x => x.ParentId == parentId).Select(x => x.Sort).ToListAsync();
                var sort = menus.Any() ? menus.Max() : 0;
                data.Sort = sort;
            }
            data.ParentId = parentId;

            await _db.SaveChangesAsync();
            return (true, "修改成功");
        }

        /// <summary>
        /// 删除菜单
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<(bool, string)> DeleteMenu(int id)
        {
            var menu = await _db.Menus.FirstOrDefaultAsync(x => x.Id == id);

            if (menu is null)
                return (false, $"菜单不存在,请刷新页面后重试!");

            if (await _db.Menus.AnyAsync(x => x.ParentId == id))
                return (false, $"当前菜单存在子菜单,请先删除子菜单!");

            var roleMenus = await _db.RoleMenus.Where(x => x.MId == menu.Id).ToListAsync();

            _db.Menus.Remove(menu);
            _db.RoleMenus.RemoveRange(roleMenus);
            await _db.SaveChangesAsync();
            return (true, "删除成功");
        }

        /// <summary>
        /// 获取菜单下拉列表
        /// </summary>
        /// <returns></returns>
        public async Task<List<SelectModel>> GetMenuSelect()
        {
            var menus = await _db.Menus.AsNoTracking().Where(x => x.IsEnable && x.Name != "系统主页").ToListAsync();
            var data = HandleMenu(menus, 0);
            return data;
        }


        private List<SelectModel> HandleMenu(List<Menu> menus, int parentId)
        {
            var selectMenus = new List<SelectModel>();
            var childMenus = menus.Where(x => x.ParentId == parentId).ToList();
            if (childMenus is null)
                return selectMenus;

            foreach (var menu in childMenus)
            {
                var newMenu = new SelectModel
                {
                    Label = menu.Name,
                    Value = menu.Id.ToString()
                };
                newMenu.Children = new List<SelectModel>();
                newMenu.Children = HandleMenu(menus, menu.Id);
                selectMenus.Add(newMenu);
            }
            return selectMenus;
        }

        public async Task<List<Menu>> GetMenuList()
        {
            return await _db.Menus.AsNoTracking().Where(x => x.IsEnable).ToListAsync();
        }

        /// <summary>
        /// 绑定菜单
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public async Task<bool> BindMenu(BindMenuModel model)
        {
            var roleMenus = await _db.RoleMenus.Where(x => x.RId == model.RoleId).ToListAsync();

            var removeMenus = roleMenus.Where(x => !model.MenuIds.Contains(x.MId)).ToList();

            var newMenuIds = model.MenuIds.Where(x => !removeMenus.Any(y => y.MId == x)).ToList();

            _db.RoleMenus.RemoveRange(removeMenus);
            await _db.RoleMenus.AddRangeAsync(newMenuIds.Select(x => new RoleMenu
            {
                MId = x,
                RId = model.RoleId
            }));
            await _db.SaveChangesAsync();

            return true;
        }


        /// <summary>
        /// 修改状态
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="userName"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<(bool, string)> EditStatus(int userId, string userName, int id)
        {
            var menu = await _db.Menus.FirstOrDefaultAsync(x => x.Id == id);

            if (menu is null)
                return (false, $"菜单不存在,请刷新页面后重试!");
            menu.ModifieyUserId = userId;
            menu.ModifieyUserName = userName;
            menu.ModifieyTime = DateTime.Now;
            menu.IsEnable = !menu.IsEnable;
            await _db.SaveChangesAsync();
            return (true, "修改状态成功");
        }
    }
}
