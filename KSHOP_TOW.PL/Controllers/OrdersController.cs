using KSHOP_TOW.PL.Resources;
using KSHOP_TWO.BLL.Service;
using KSHOP_TWO.DAL.DTO.Request;
using KSHOP_TWO.DAL.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using System.Security.Claims;

namespace KSHOP_TOW.PL.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class OrdersController : ControllerBase
    {
        private readonly IOrderService _orderService;
        private readonly IStringLocalizer<SharedResources> _localizer;

        public OrdersController(IOrderService orderService, IStringLocalizer<SharedResources> localizer)
        {
            _orderService = orderService;
            _localizer = localizer; 
        }

        [HttpGet("")]
        
        public async Task<IActionResult> GetMyOrders()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var orders = await _orderService.GetOrders(userId);
            return Ok(new {data = orders});
        }

        [HttpGet("{id}")]
       
        public async Task<IActionResult> GetUserOrders(int id)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            
            var order = await _orderService.GetUserOrder(userId, id);
            return Ok(new { data = order});
        }

        [HttpGet("admin")]

        public async Task<IActionResult> GetAllOrders([FromQuery] OrderStatusEnum status = OrderStatusEnum.Pending)
        {
            
            var order = await _orderService.GetAllOrders(status);
            return Ok(new { data = order });
        }


        [HttpPatch("admin/{id}/status")]

        public async Task<IActionResult> UpdateStatesOrders(int id,[FromBody] ChangeOrderStatusRequest status)
        {

            var order = await _orderService.ChangeOrderStatus(id,status);
            if (!order) return BadRequest();
            return Ok(new { data = order });
        }
    }



}
