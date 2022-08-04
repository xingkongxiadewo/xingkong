using Auth.DTO.SearchModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Auth.DTO.SeacrhModel.Matter
{
    public class MatterSearchModel : BaseSearchModel
    {
        public int MatterType { get; set; }
    }
}
