using KSHOP_TWO.DAL.DTO.Request;
using KSHOP_TWO.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KSHOP_TWO.DAL.DTO.Response
{
    public class OrderResponse
    {
        public int Id { get; set; }

        public string City { get; set; } = null!;
        public string Street { get; set; } = null!;

        public string PhoneNumber { get; set; }

        public decimal AmountPaid { get; set; }

        public OrderStatusEnum OrderStatus { get; set; }

        public PaymentMethodEnum PaymentMethod { get; set; }

        public DateTime OrderDate { get; set; }

        public List<OrderItemsResponse> OrderItems { get; set; } = new List<OrderItemsResponse>();


    }
}
