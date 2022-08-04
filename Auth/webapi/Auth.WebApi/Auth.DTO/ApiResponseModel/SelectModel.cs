using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Auth.DTO.ApiResponseModel
{
    public class SelectModel
    {
        public string Value { get; set; }
        public string Label { get; set; }
        public List<SelectModel> Children  { get; set; }
}
}
