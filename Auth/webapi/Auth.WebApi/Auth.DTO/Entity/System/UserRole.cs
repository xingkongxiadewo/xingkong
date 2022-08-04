using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Auth.DTO.Entity.System
{
    [Table("t_user_role")]
    public class UserRole
    {
        [Key]
        public int Id { get; set; }
        /// <summary>
        /// 角色Id
        /// </summary>
        public int RId { get; set; }
        /// <summary>
        /// 用户Id
        /// </summary>
        public int UserId { get; set; }

        [ForeignKey(nameof(RId))]
        public Role Role { get; set; }
    }
}
