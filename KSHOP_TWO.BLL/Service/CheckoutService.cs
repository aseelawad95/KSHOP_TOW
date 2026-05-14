using KSHOP_TWO.DAL.DTO.Request;
using KSHOP_TWO.DAL.DTO.Response;
using KSHOP_TWO.DAL.Models;
using KSHOP_TWO.DAL.Repository;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Stripe.Checkout;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KSHOP_TWO.BLL.Service
{
    public class CheckoutService : ICheckoutService
    {

        private readonly ICartRepository _cartRepository;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IOrderRepository _orderRepository;
        private readonly ICartService _cartService;
        private readonly IProductRepository _productRepository;
        private readonly IEmailSender _emailSender;
        public CheckoutService(ICartRepository cartRepository, UserManager<ApplicationUser> userManager, IHttpContextAccessor httpContextAccessor, IOrderRepository orderRepository,
                 ICartService cartService,
                    IProductRepository productRepository,
                     IEmailSender emailSender
                 )
        {
            _cartRepository = cartRepository;
            _userManager = userManager;
            _httpContextAccessor = httpContextAccessor;
            _orderRepository = orderRepository;
            _cartService = cartService;
            _productRepository = productRepository;
            _emailSender = emailSender;

        }
     
        public async Task<CheckoutResponse> ProcessCheckout(string UserId, CheckoutRequest request)
        {
            var cartItems = await _cartRepository.GetAllAsync(filter: c => c.UserId == UserId,
                includes: new[] { nameof(Cart.Product),
                 $"{nameof(Cart.Product)}.{nameof(Product.Translations)}"
                }
                );
            if (!cartItems.Any())
            {
                return new CheckoutResponse
                {
                    Success = false,
                    Error = "Cart is empty."
                };
            }
            var user = await _userManager.FindByIdAsync(UserId);

            var city = request.City ?? user.City;
            if (city is null)
            {
                return new CheckoutResponse
                {
                    Success = false,
                    Error = "City is required."
                };
            }


            var street = request.Street ?? user.Street;
            if (street is null)
            {
                return new CheckoutResponse
                {
                    Success = false,
                    Error = "Street is required."
                };
            }


            var phoneNumber = request.PhoneNumber ?? user.PhoneNumber;

            if (phoneNumber is null)
            {
                return new CheckoutResponse
                {
                    Success = false,
                    Error = "Phone number is required."
                };
            }
            foreach (var item in cartItems)
            {
                if (item.Count > item.Product.Quantity)
                {
                    return new CheckoutResponse
                    {
                        Success = false,
                        Error = $"Not enough stock for product {item.Product.Quantity}."
                    };
                }

            }
            var order = new Order
            {
                UserId = UserId,
                City = city,
                Street = street,
                PhoneNumber = phoneNumber,
                AmountPaid = cartItems.Sum(c => c.Product.Price * c.Count),
                PaymentMethod = request.paymentMethod,
                OrderItems = cartItems.Select(c => new OrderItem
                {
                    ProductId = c.ProductId,
                    Quantity = c.Count,
                    UnitPrice = c.Product.Price,
                    TotalPrice = c.Product.Price * c.Count
                }).ToList()
            };

            await _orderRepository.CreateAsync(order);

            if (request.paymentMethod == PaymentMethodEnum.Cash)
            {
                return new CheckoutResponse
                {
                    Success = true,

                };
            }
            if (request.paymentMethod == PaymentMethodEnum.Visa)
            {
                var options = new SessionCreateOptions
                {
                    PaymentMethodTypes = new List<string> { "card" },
                    Mode = "payment",
                    SuccessUrl = $"{_httpContextAccessor.HttpContext.Request.Scheme}://{_httpContextAccessor.HttpContext.Request.Host}/api/checkouts/success?sessionId={{CHECKOUT_SESSION_ID}}",
                    CancelUrl = $"{_httpContextAccessor.HttpContext.Request.Scheme}://{_httpContextAccessor.HttpContext.Request.Host}/checkout/cancel",

                    LineItems = new List<SessionLineItemOptions>()
                };

                foreach (var item in cartItems)
                {
                    options.LineItems.Add(
                         new SessionLineItemOptions
                         {
                             PriceData = new SessionLineItemPriceDataOptions
                             {
                                 Currency = "USD",
                                 ProductData = new SessionLineItemPriceDataProductDataOptions
                                 {
                                     Name = item.Product.Translations.FirstOrDefault(t => t.Language == "en").Name,

                                 },
                                 UnitAmount = (long)(item.Product.Price * 100),
                             },
                             Quantity = item.Count,
                         }
                        );
                }
                var service = new SessionService();
                var session = service.Create(options);
                order.StripeSessionId = session.Id;

                await _orderRepository.UpdateAsync(order);

                return new CheckoutResponse
                {
                    Success = true,
                    StripeUrl = session.Url
                };

            }
            return new CheckoutResponse
                {
                Success = false,
                Error = "Invalid payment method."
            };
                
        }

        public async Task<CheckoutResponse> HandlePaymentSuccess(string sessionId)
        {
            var order = await _orderRepository.GetOne(o => o.StripeSessionId == sessionId,
                includes: new[] { nameof(Order.OrderItems),
                    $"{nameof(Order.OrderItems)}.{nameof(OrderItem.Product)}",
                    $"{nameof(Order.OrderItems)}.{nameof(OrderItem.Product)}.{nameof(Product.Translations)}"

                }
                );

            order.OrderStatus = OrderStatusEnum.Paid;
            await _orderRepository.UpdateAsync(order);

            await _cartService.ClearCart(order.UserId);
            var user = await _userManager.FindByIdAsync(order.UserId);
            await _emailSender.SendEmailAsync(user.Email, "Order Confirmation", $"Your order with ID {order.Id} has been successfully placed.");
            var isLowStock = await _productRepository.DecreaseQuantityAsync(order.OrderItems);
            if (isLowStock != null)
            {
                return new CheckoutResponse
                {
                    Success = true,
                    OrderId = order.Id
                };
            }
            foreach (var item in isLowStock)
            {
               if(isLowStock != null)
                {
                    await _emailSender.SendEmailAsync("aseelawad252@gmail.com", "Low Stock Alert",
                   $"<h2>Product {item.Translations.FirstOrDefault(t => t.Language == "en").Name} Current Quantity : {item.Quantity}</h2>");
                } 
               
            }

            return new CheckoutResponse
            {
                Success = true,
                OrderId = order.Id
            };
        }
    }


}

