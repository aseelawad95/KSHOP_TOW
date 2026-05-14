using KSHOP_TWO.DAL.DTO.Request;
using KSHOP_TWO.DAL.DTO.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KSHOP_TWO.BLL.Service
{
    public interface ICheckoutService
    {
        Task<CheckoutResponse> ProcessCheckout(string UserId, CheckoutRequest request);
        Task<CheckoutResponse> HandlePaymentSuccess(string sessionId);
    }
}
