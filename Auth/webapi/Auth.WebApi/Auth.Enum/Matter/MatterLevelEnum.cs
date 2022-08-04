using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Auth.Enum.Matter
{
    public enum MatterLevelEnum
    {
        /// <summary>
        /// 紧急
        /// </summary>
        [Description("紧急")]
        Urgent=1,
        /// <summary>
        /// 重要
        /// </summary>
        [Description("重要")]
        Serious =2,
        /// <summary>
        /// 一般
        /// </summary>
        [Description("一般")]
        Commonly =3,

    }
}
