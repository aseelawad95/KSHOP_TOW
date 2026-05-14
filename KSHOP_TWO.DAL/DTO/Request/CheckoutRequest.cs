using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace KSHOP_TWO.DAL.DTO.Request
{
    public enum PaymentMethodEnum
    {
        Cash = 1,
        Visa = 2,
    }
    public class CheckoutRequest
    {
        public string? Street { get; set; }
        public string? PhoneNumber { get; set; }
        public string? City { get; set; }

        [JsonConverter(typeof(JsonStringEnumConverter))]
        public PaymentMethodEnum paymentMethod { get; set; }
        
    }
}
