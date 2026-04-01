using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KSHOP_TWO.DAL.DTO.Response
{
    public class RegisterResponse
    {
        public string Message { get; set; }
        public bool Success { get; set; }

        public List<string> Errors { get; set; }
    }
}
