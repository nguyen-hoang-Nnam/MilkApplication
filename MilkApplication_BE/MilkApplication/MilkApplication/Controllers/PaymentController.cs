using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MilkApplication.BLL.Service;
using MilkApplication.BLL.Service.IService;
using MilkApplication.DAL.Data;
using MilkApplication.DAL.enums;
using MilkApplication.DAL.Models;
using MilkApplication.DAL.Models.DTO;
using Net.payOS.Types;

namespace MilkApplication.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PaymentController : ControllerBase
    {
        private readonly IPaymentService _paymentService;
        private readonly IOrderService _orderService;
        private readonly SignatureVerifier _signatureVerifier;
        private readonly AppDbContext _context;

        public PaymentController(IPaymentService paymentService, IOrderService orderService, SignatureVerifier signatureVerifier, AppDbContext context)
        {
            _paymentService = paymentService;
            _orderService = orderService;
            _signatureVerifier = signatureVerifier;
            _context = context;
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


        [HttpPost("confirm-payment")]
        public async Task<IActionResult> ConfirmPayment([FromBody] PaymentConfirmationRequest request)
        {
            if (request == null || string.IsNullOrWhiteSpace(request.TransactionId))
            {
                return BadRequest("Invalid request.");
            }

            try
            {
                var result = await _paymentService.ConfirmPaymentAsync(request.TransactionId);
                if (result.IsSucceed)
                {
                    return Ok(result.Message);
                }

                return BadRequest(result.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Internal server error.");
            }
        }

        [HttpGet("totalAmountByDay")]
        public async Task<IActionResult> GetTotalAmountByDay(DateTime date)
        {
            var totalAmount = await _paymentService.GetTotalAmountByDayAsync(date);
            return Ok(totalAmount);
        }

        [HttpGet("totalAmountByMonth")]
        public async Task<IActionResult> GetTotalAmountByMonth(int year, int month)
        {
            var totalAmount = await _paymentService.GetTotalAmountByMonthAsync(year, month);
            return Ok(totalAmount);
        }

        [HttpGet("totalAmountsForLast12Months")]
        public async Task<IActionResult> GetTotalAmountsForLast12Months()
        {
            var totalAmounts = await _paymentService.GetTotalAmountsForLast12MonthsAsync();
            return Ok(totalAmounts);
        }

        [HttpGet("Payment/{userId}")]
        public async Task<IActionResult> GetPaymentsByUserId(string userId)
        {
            try
            {
                var response = await _orderService.GetPaymentsByUserIdAsync(userId);

                if (response.IsSucceed)
                    return Ok(response.Data);

                return NotFound(response.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }
    }
}
