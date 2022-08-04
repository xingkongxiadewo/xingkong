using Auth.DTO.ApiResponseModel.Role;
using Auth.DTO.Common;
using Auth.DTO.SeacrhModel.Role;
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
    public class RoleApiController : ControllerBase
    {
        private readonly IRoleService _roleService;
        private readonly IUserRoleService _userRoleService;
        private readonly IMapper _mapper;

        public RoleApiController(IRoleService roleService, IUserRoleService userRoleService, IMapper mapper)
        {
            _roleService = roleService;
            _userRoleService = userRoleService;
            _mapper = mapper;
        }

        [HttpPost]
        public async Task<object> GetRolePageList([FromBody] RoleSearchModel model)
        {
            var (total, data) = await _roleService.GetRolePageList(model);

            var result = new ResponseModel<List<RoleModel>>()
            {
                Count = total,
                Data = new List<RoleModel>()
            };
            result.Data.AddRange(data.Select(x => _mapper.Map<RoleModel>(x)));

            return result;
        }

        [HttpPost]
        public async Task<object> AddRole([FromBody] AddRoleModel model)
        {
            var (result, msg) = await _roleService.AddRole(int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)), User.FindFirstValue(ClaimTypes.GivenName), model);

            return new ResponseModel()
            {
                Code = result ? ResponseCode.Success : ResponseCode.Error,
                Message = msg
            };
        }

        [HttpPost]
        public async Task<object> EditRole([FromBody] EditRoleModel model)
        {
            var (retult, msg) = await _roleService.EditRole(int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)), User.FindFirstValue(ClaimTypes.GivenName), model);

            return new ResponseModel()
            {
                Code = retult ? ResponseCode.Success : ResponseCode.Error,
                Message = msg
            };
        }

        [HttpPost]
        public async Task<object> DeleteRole(int id)
        {
            var (result, msg) = await _roleService.DeleteRole(id);

            return new ResponseModel()
            {
                Code = result ? ResponseCode.Success : ResponseCode.Error,
                Message = msg
            };
        }

        [HttpGet]
        public async Task<object> GetBoundRoles(int userId)
        {
            var userRoles = await _userRoleService.GetRolesByUserId(userId);
            var result = new ResponseModel<List<RoleModel>>()
            {
                Code = ResponseCode.Success,
                Data = new List<RoleModel>()
            };
            result.Data.AddRange(userRoles.Select(x => _mapper.Map<RoleModel>(x)));
            return result;
        }

        [HttpGet]
        public async Task<object> GetRoleList()
        {
            var roles = await _roleService.GetRoleList();
            var result = new ResponseModel<List<RoleModel>>()
            {
                Code = ResponseCode.Success,
                Data = new List<RoleModel>()
            };
            result.Data.AddRange(roles.Select(x => _mapper.Map<RoleModel>(x)));
            return result;
        }

        [HttpPost]
        public async Task<object> BindRole(BindRoleModel model)
        {
            var result = await _userRoleService.BindRole(model);

            return new ResponseModel()
            {
                Code = result ? ResponseCode.Success : ResponseCode.Error,
                Message = result ? "绑定成功" : "绑定失败"
            };
        }

        [HttpPost]
        public async Task<object> EditStatus(int id)
        {
            var (result, msg) = await _roleService.EditStatus(int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)), User.FindFirstValue(ClaimTypes.GivenName), id);
            return new ResponseModel()
            {
                Code = result ? ResponseCode.Success : ResponseCode.Error,
                Message = msg
            };
        }
    }
}
