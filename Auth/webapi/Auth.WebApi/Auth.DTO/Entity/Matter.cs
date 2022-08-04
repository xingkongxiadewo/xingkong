using Auth.Enum.Matter;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Auth.DTO.Entity
{
    [Table("t_matter")]
    public class Matter : BaseEntity
    {
        [Key]
        public int Id { get; set; }
        
        /// <summary>
        /// 事项
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// 级别
        /// </summary>
        public MatterLevelEnum Level { get; set; }

        /// <summary>
        /// 类型
        /// </summary>
        public MatterTypeEnum Type { get; set; }
    }
}
