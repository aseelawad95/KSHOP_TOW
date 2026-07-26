using KSHOP_TWO.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace KSHOP_TWO.DAL.DTO.Request
{
    public class ChangeOrderStatusRequest
    {
        [JsonConverter(typeof(JsonStringEnumConverter))]
        public OrderStatusEnum Status { get; set; }
    }
}
