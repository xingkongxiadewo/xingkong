using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Auth.Enum.Matter
{
    public enum MatterTypeEnum
    {
        /// <summary>
        /// 待办事项
        /// </summary>
        [Description("待办事项")]
        NeedMatter = 1,
        /// <summary>
        /// 已办事项
        /// </summary>
        [Description("已办事项")]
        DoneMatter = 2,
        /// <summary>
        /// 垃圾箱
        /// </summary>
        [Description("垃圾箱")]
        Dustbin = 3
    }
}
