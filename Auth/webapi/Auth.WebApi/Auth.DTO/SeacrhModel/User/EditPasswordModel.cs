using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Auth.DTO.SeacrhModel.User
{
    public class EditPasswordModel
    {
        public string OldPassword { get; set; }
        public string NewPassword { get; set; } 
    }
}
