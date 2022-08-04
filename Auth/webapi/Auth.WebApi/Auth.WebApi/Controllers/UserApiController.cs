using Auth.DTO.ApiResponseModel.User;
using Auth.DTO.Common;
using Auth.DTO.SeacrhModel.User;
using Auth.DTO.SearchModel.User;
using Auth.Service.IService.System;
using AutoMapper;
using IdentityModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Auth.WebApi.Controllers
{
    [Authorize]
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class UserApiController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly IMapper _mapper;

        public UserApiController(IUserService userService, IMapper mapper)
        {
            _userService = userService;
            _mapper = mapper;
        }

        [HttpPost]
        public async Task<object> GetUserPageList([FromBody] UserSearchModel model)
        {
            var (total, data) = await _userService.GetUserPageList(model);

            var result = new ResponseModel<List<UserModel>>()
            {
                Count = total,
                Data = new List<UserModel>()
            };
            result.Data.AddRange(data.Select(x => _mapper.Map<UserModel>(x)));

            return result;
        }

        [HttpPost]
        public async Task<object> AddUser([FromBody] AddUserModel model)
        {
            var (result, msg) = await _userService.AddUser(int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)), User.FindFirstValue(ClaimTypes.GivenName), model);
            return new ResponseModel()
            {
                Code = result ? ResponseCode.Success : ResponseCode.Error,
                Message = msg
            };
        }

        [HttpPost]
        public async Task<object> EditUser([FromBody] EditUserModel model)
        {
            var (result, msg) = await _userService.EditUser(int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)), User.FindFirstValue(ClaimTypes.GivenName), model);
            return new ResponseModel()
            {
                Code = result ? ResponseCode.Success : ResponseCode.Error,
                Message = msg
            };
        }

        [HttpPost]
        public async Task<object> DeleteUser(int id)
        {
            var result = await _userService.DeleteUser(id);
            return new ResponseModel()
            {
                Code = result ? ResponseCode.Success : ResponseCode.Error,
                Message = result ? "删除成功" : "删除失败，请联系管理员!"
            };
        }

        [HttpPost]
        public async Task<object> EditPassword(EditPasswordModel model)
        {
            var (result, msg) = await _userService.EditPassword(int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)), User.FindFirstValue(ClaimTypes.GivenName), model);
            return new ResponseModel()
            {
                Code = result ? ResponseCode.Success : ResponseCode.Error,
                Message = msg
            };
        }

        [HttpPost]
        public async Task<object> EditStatus(int id) 
        {
            var (result, msg) = await _userService.EditStatus(int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)), User.FindFirstValue(ClaimTypes.GivenName), id);
            return new ResponseModel()
            {
                Code = result ? ResponseCode.Success : ResponseCode.Error,
                Message = msg
            };
        }
    }
}
