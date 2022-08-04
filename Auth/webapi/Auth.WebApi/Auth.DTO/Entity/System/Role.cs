using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Auth.DTO.Entity.System
{
    [Table("t_role")]
    public class Role : BaseEntity
    {
        [Key]
        public int Id { get; set; }
        /// <summary>
        /// 角色名称
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 是否启用
        /// </summary>

        public bool IsEnable { get; set; }
        public List<UserRole> UserRoles { get; set; }
    }
}
