using Auth.DTO.Common;
using Auth.DTO.Entity;
using Auth.DTO.SeacrhModel.Matter;
using Auth.Service.IService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Auth.WebApi.Controllers
{
    [Authorize]
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class MatterApiController : ControllerBase
    {
        private readonly IMatterService _matterService;

        public MatterApiController(IMatterService matterService)
        {
            _matterService = matterService;
        }

        [HttpPost]
        public async Task<object> GetMatterPageList([FromBody] MatterSearchModel model)
        {
            var (total, data) = await _matterService.GetMatterPageList(int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)), model);
            var result = new ResponseModel<List<Matter>>()
            {
                Count = total,
                Data = new List<Matter>()
            };
            result.Data.AddRange(data);
            return result;
        }

        [HttpPost]
        public async Task<object> AddMatter([FromBody] AddMatterModel model)
        {
            var result = await _matterService.AddMatter(int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)), User.FindFirstValue(ClaimTypes.GivenName), model);
            return new ResponseModel()
            {
                Code = result ? ResponseCode.Success : ResponseCode.Error,
                Message = result ? "新增成功" : "新增失败"
            };
        }

        [HttpPost]
        public async Task<object> EditMatter([FromBody] EditMatterModel model)
        {
            var result = await _matterService.EditMatter(int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)), User.FindFirstValue(ClaimTypes.GivenName), model);
            return new ResponseModel()
            {
                Code = result ? ResponseCode.Success : ResponseCode.Error,
                Message = result ? "修改成功" : "修改失败"
            };
        }


        /// <summary>
        /// 垃圾箱处理
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="userName"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<object> DustbinHandle(int id, int type)
        {
            var result = await _matterService.DustbinHandle(int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)), User.FindFirstValue(ClaimTypes.GivenName), id, type);
            return new ResponseModel()
            {
                Code = result ? ResponseCode.Success : ResponseCode.Error,
                Message = result ? "处理成功" : "处理失败"
            };
        }
    }
}
