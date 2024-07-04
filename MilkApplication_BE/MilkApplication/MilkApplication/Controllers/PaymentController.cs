using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MilkApplication.BLL.Service.IService;
using MilkApplication.DAL.Models;
using MilkApplication.DAL.Models.DTO;

namespace MilkApplication.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PaymentController : ControllerBase
    {
        private readonly IPaymentService _paymentService;
        private readonly IOrderService _orderService;

        public PaymentController(IPaymentService paymentService, IOrderService orderService)
        {
            _paymentService = paymentService;
            _orderService = orderService;
        }
        [HttpPost("Create-Payment-Link")]
        public async Task<IActionResult> CreatePaymentLink([FromBody] CreatePaymentLinkRequest request)
        {
            if (request == null)
            {
                return BadRequest("Invalid request");
            }

            var order = await _orderService.GetOrderEntityByIdAsync(request.orderId);
            if (order == null)
            {
                return NotFound("Order not found");
            }

            var result = await _paymentService.CreatePaymentLink(order, request.CancelUrl, request.ReturnUrl);

            if (result.Success)
            {
                return Ok(result);
            }

            return BadRequest(result.ErrorMessage);
        }
    }
}
