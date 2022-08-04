using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Auth.DTO.ApiResponseModel.Role
{
    public class BindingRoleModel
    {
        /// <summary>
        /// 角色Id
        /// </summary>
        public long Id { get; set; }
        /// <summary>
        /// 角色名称
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 是否已经绑定
        /// </summary>
        public bool IsBind { get; set; }
    }
}
