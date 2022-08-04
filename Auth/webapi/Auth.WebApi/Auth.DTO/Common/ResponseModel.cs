using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Auth.DTO.Common
{

    public class ResponseModel
    {
        /// <summary>
        /// 错误码
        /// </summary>
        public ResponseCode Code { get; set; } = ResponseCode.Success;
        /// <summary>
        /// 错误内容
        /// </summary>
        public string Message { get; set; }
    }

    public class ResponseModel<T> : ResponseModel
    {
        /// 总数
        /// </summary>
        public int Count { get; set; } = 0;

        /// <summary>
        /// 自定义内容
        /// </summary>

        public T Data { get; set; }
    }

    public enum ResponseCode
    {
        Success = 1,
        Error = 2,
        UnAuth = 3,
        UnLogin = 4
    }

}
