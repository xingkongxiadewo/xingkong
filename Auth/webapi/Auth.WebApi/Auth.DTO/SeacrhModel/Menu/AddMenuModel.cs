using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Auth.DTO.SeacrhModel.Menu
{
    public class AddMenuModel
    {
        public string Name { get; set; }
        public string Url { get; set; }
        public string? Icon { get; set; }
        public int? ParentId { get; set; }
    }
}
