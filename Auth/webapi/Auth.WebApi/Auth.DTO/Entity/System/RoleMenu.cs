using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Auth.DTO.Entity.System
{
    [Table("t_role_menu")]
    public class RoleMenu
    {
        [Key]
        public int Id { get; set; }
        /// <summary>
        /// 角色Id
        /// </summary>
        public int RId { get; set; }
        /// <summary>
        /// 菜单Id
        /// </summary>
        public int MId { get; set; }

        [ForeignKey(nameof(MId))]
        public Menu Menu { get; set; }
    }
}
