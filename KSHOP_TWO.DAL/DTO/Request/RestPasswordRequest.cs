using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KSHOP_TWO.DAL.DTO.Request
{
    public class RestPasswordRequest
    {
        public string Code { get; set; }

        public string NewPassword { get; set; }

        public string Email { get; set; }
    }
}
