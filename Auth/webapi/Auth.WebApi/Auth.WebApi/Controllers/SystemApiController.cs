using Auth.DTO.Common;
using Auth.DTO.SearchModel;
using Auth.Service.IService.System;
using Auth.WebApi.Model;
using IdentityModel;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Auth.WebApi.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class SystemApiController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly IUserRoleService _userRoleService;
        private readonly IMenuService _meunService;
        private readonly IOptions<JwtSettings> _jwtSettings;

        public SystemApiController(IUserService userService, IUserRoleService userRoleService, IMenuService meunService, IOptions<JwtSettings> jwtSettings)
        {
            _userService = userService;
            _userRoleService = userRoleService;
            _meunService = meunService;
            _jwtSettings = jwtSettings;
        }

        [AllowAnonymous]
        [HttpPost]
        public async Task<object> Login([FromBody] LoginModel input)
        {
            var result = new ResponseModel<object>();
            if (string.IsNullOrEmpty(input.Account))
            {
                result.Code = ResponseCode.Error;
                result.Message = "请先输入账号!";
                return result;
            }

            if (string.IsNullOrEmpty(input.Password))
            {
                result.Code = ResponseCode.Error;
                result.Message = "请先输入密码!";
                return result;
            }

            var user = await _userService.GetUserByLogin(input.Account, input.Password);

            if (user is null)
            {
                result.Code = ResponseCode.Error;
                result.Message = "账户或者密码输入错误!";
                return result;
            }
            var tokenHandler = new JwtSecurityTokenHandler();

            var key = Encoding.UTF8.GetBytes(_jwtSettings.Value.SecretKey);
            var authTime = DateTime.Now;//授权时间
            var expiresAt = authTime.AddDays(30);//过期时间
            var claimIdentity = new ClaimsIdentity(CookieAuthenticationDefaults.AuthenticationScheme);
            claimIdentity.AddClaim(new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()));
            claimIdentity.AddClaim(new Claim(ClaimTypes.Name, user.Account));
            claimIdentity.AddClaim(new Claim(ClaimTypes.GivenName, user.Name));
            claimIdentity.AddClaim(new Claim(ClaimTypes.Email, user.Email));
            claimIdentity.AddClaim(new Claim(JwtClaimTypes.Audience, _jwtSettings.Value.Audience));
            claimIdentity.AddClaim(new Claim(JwtClaimTypes.Issuer, _jwtSettings.Value.Issuer));


            var tokenDescripor = new SecurityTokenDescriptor
            {
                Subject = claimIdentity,
                Expires = expiresAt,
                //对称秘钥SymmetricSecurityKey
                //签名证书(秘钥，加密算法)SecurityAlgorithms
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };



            var token = tokenHandler.CreateToken(tokenDescripor);
            var tokenString = tokenHandler.WriteToken(token);
            var data = new
            {
                access_token = tokenString,
                token_type = "Bearer",
                profile = new
                {
                    id = user.Id,
                    name = user.Name,
                    auth_time = authTime,
                    expires_at = expiresAt
                }
            };

            result.Message = "登入成功";
            result.Data = data;
            return result;
        }

        [Authorize]
        [HttpPost]
        public async Task<object> Menus()
        {
            var data = new ResponseModel<List<object>> { Data = new List<object>() };
            var account = User.FindFirstValue(ClaimTypes.Name);
            var user = await _userService.GetUserByAccount(account);
            var userRoles = await _userRoleService.GetRolesByUserId(int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)));
            var userRIds = userRoles.Select(x => x.Id).ToList();
            var menus = await _meunService.GetMenuByRoleId(userRIds);
            var rootMenus = menus.Where(x => x.ParentId == 0).OrderBy(x => x.Sort).ToList();

            foreach (var item1 in rootMenus)
            {
                var children1 = new List<object>();
                var menus2 = menus.Where(x => x.ParentId == item1.Id).OrderBy(x => x.Sort).ToList();
                foreach (var item2 in menus2)
                {
                    if (!string.IsNullOrWhiteSpace(item2.Url))
                    {
                        children1.Add(new { icon = item2.Icon, index = item2.Url, title = item2.Name });
                        continue;
                    }

                    var menus3 = menus.Where(x => x.ParentId == item2.Id).OrderBy(x => x.Sort).ToList();
                    if (menus3.Count == 0)
                        continue;

                    var children2 = new List<object>();
                    menus3.ForEach(item3 => children2.Add(new { icon = item3.Icon, index = item3.Url, title = item3.Name }));
                    children1.Add(new { icon = item2.Icon, index = item2.Url, title = item2.Name, children = children2 });
                }
                data.Data.Add(new { icon = item1.Icon, index = item1.Url, title = item1.Name, children = children1 });
            }
            return data;
        }

        [Authorize]
        [HttpGet]
        public async Task<object> GetUser()
        {
            var user = await _userService.GetUserById(int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)));
            if (user is null)
                return new ResponseModel()
                {
                    Code = ResponseCode.Error,
                    Message = "当前登录用户不存在!",
                };


            return new ResponseModel<object>()
            {
                Code = ResponseCode.Success,
                Message = "获取成功",
                Data = new
                {
                    user.Account,
                    user.Email,
                    user.Name,
                    user.Phone
                }
            };

        }
    }
}
