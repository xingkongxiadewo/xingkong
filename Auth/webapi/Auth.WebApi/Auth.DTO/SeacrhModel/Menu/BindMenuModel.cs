using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Auth.DTO.SeacrhModel.Menu
{
    public class BindMenuModel
    {
        public int RoleId { get; set; }
        public List<int> MenuIds { get; set; }
    }
}
