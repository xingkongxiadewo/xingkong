using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Auth.DTO.SeacrhModel.Role
{
    public class BindRoleModel
    {
        public int UserId { get; set; }
        public List<int> RoleIds { get; set; }
    }
}
