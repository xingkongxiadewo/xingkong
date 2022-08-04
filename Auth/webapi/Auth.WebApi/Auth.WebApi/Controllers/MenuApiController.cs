using Auth.DTO.ApiResponseModel;
using Auth.DTO.ApiResponseModel.Menu;
using Auth.DTO.Common;
using Auth.DTO.SeacrhModel.Menu;
using Auth.Service.IService.System;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Auth.WebApi.Controllers
{
    [Authorize]
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class MenuApiController : ControllerBase
    {
        private readonly IMenuService _menuService;
        private readonly IMapper _mapper;

        public MenuApiController(IMenuService menuService, IMapper mapper)
        {
            _menuService = menuService;
            _mapper = mapper;
        }


        [HttpPost]
        public async Task<object> GetMenuPageList([FromBody] MenuSearchModel model)
        {
            var (total, data) = await _menuService.GetMenuPageList(model);
            var result = new ResponseModel<List<MenuModel>>()
            {
                Count = total,
                Data = new List<MenuModel>()
            };
            result.Data.AddRange(data.Select(x => _mapper.Map<MenuModel>(x)));
            return result;
        }

        [HttpPost]
        public async Task<object> AddMenu([FromBody] AddMenuModel model)
        {
            var (result, msg) = await _menuService.AddMenu(int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)), User.FindFirstValue(ClaimTypes.GivenName), model);
            return new ResponseModel()
            {
                Code = result ? ResponseCode.Success : ResponseCode.Error,
                Message = msg
            };
        }

        [HttpPost]
        public async Task<object> EditMenu([FromBody] EditMenuModel model)
        {
            var (result, msg) = await _menuService.EditMenu(int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)), User.FindFirstValue(ClaimTypes.GivenName), model);
            return new ResponseModel()
            {
                Code = result ? ResponseCode.Success : ResponseCode.Error,
                Message = msg
            };
        }

        [HttpPost]
        public async Task<object> DeleteMenu(int id)
        {
            var (result, msg) = await _menuService.DeleteMenu(id);
            return new ResponseModel()
            {
                Code = result ? ResponseCode.Success : ResponseCode.Error,
                Message = msg
            };
        }

        [HttpPost]
        public async Task<object> GetMenuSelect()
        {
            var result = await _menuService.GetMenuSelect();

            return new ResponseModel<List<SelectModel>>
            {
                Code = ResponseCode.Success,
                Count = result.Count,
                Data = result
            };
        }

        [HttpGet]
        public async Task<object> GetMenuList()
        {
            var data = new ResponseModel<List<object>> { Data = new List<object>() };

            var menus = await _menuService.GetMenuList();
            var rootMenus = menus.Where(x => x.ParentId == 0).OrderBy(x => x.Sort).ToList();

            foreach (var item1 in rootMenus)
            {
                var children1 = new List<object>();
                var menus2 = menus.Where(x => x.ParentId == item1.Id).OrderBy(x => x.Sort).ToList();
                foreach (var item2 in menus2)
                {
                    if (!string.IsNullOrWhiteSpace(item2.Url))
                    {
                        children1.Add(new { id = item2.Id, label = item2.Name });
                        continue;
                    }

                    var menus3 = menus.Where(x => x.ParentId == item2.Id).OrderBy(x => x.Sort).ToList();
                    if (menus3.Count == 0)
                        continue;

                    var children2 = new List<object>();
                    menus3.ForEach(item3 => children2.Add(new { id = item3.Id, label = item3.Name }));
                    children1.Add(new { id = item2.Id, label = item2.Name, children = children2 });
                }
                data.Data.Add(new { id = item1.Id, label = item1.Name, children = children1 });
            }
            return data;
        }

        [HttpGet]
        public async Task<object> GetMenuListByRoleId(int roleId)
        {
            var data = await _menuService.GetMenuByRoleId(new List<int> { roleId });
            var result = new ResponseModel<List<MenuModel>>()
            {
                Count = data.Count,
                Data = new List<MenuModel>()
            };
            result.Data.AddRange(data.Select(x => _mapper.Map<MenuModel>(x)));
            return result;
        }

        [HttpPost]
        public async Task<object> BindMenu(BindMenuModel model)
        {
            var result = await _menuService.BindMenu(model);

            return new ResponseModel()
            {
                Code = result ? ResponseCode.Success : ResponseCode.Error,
                Message = result ? "绑定成功" : "绑定失败"
            };
        }
        [HttpPost]
        public async Task<object> EditStatus(int id)
        {
            var (result, msg) = await _menuService.EditStatus(int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)), User.FindFirstValue(ClaimTypes.GivenName), id);

            return new ResponseModel()
            {
                Code = result ? ResponseCode.Success : ResponseCode.Error,
                Message = msg
            };
        }
    }
}
