using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Auth.DTO.ApiResponseModel.Menu
{
    public class MenuModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string? Icon { get; set; }
        public string Url { get; set; }
        public int Sort { get; set; }
        public int ParentId { get; set; }
        public bool IsEnable { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreateTime { get; set; }
        /// <summary>
        /// 创建人名称
        /// </summary>
        public string CreateUserName { get; set; }
        /// <summary>
        /// 创建人Id
        /// </summary>
        public int CreateUserId { get; set; }
        /// <summary>
        /// 修改时间
        /// </summary>
        public DateTime ModifieyTime { get; set; }
        /// <summary>
        /// 修改人名称
        /// </summary>
        public string ModifieyUserName { get; set; }
        /// <summary>
        /// 修改人Id
        /// </summary>
        public int ModifieyUserId { get; set; }
    }
}
