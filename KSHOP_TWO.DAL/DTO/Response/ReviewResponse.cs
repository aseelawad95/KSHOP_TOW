using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KSHOP_TWO.DAL.DTO.Response
{
    public class ReviewResponse
    {
        public string Comment { get; set; }
        public int Rate { get; set; }
        public string UserName { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}

