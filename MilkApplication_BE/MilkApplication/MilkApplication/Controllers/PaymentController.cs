using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MilkApplication.BLL.Service;
using MilkApplication.BLL.Service.IService;
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

        public PaymentController(IPaymentService paymentService, IOrderService orderService, SignatureVerifier signatureVerifier)
        {
            _paymentService = paymentService;
            _orderService = orderService;
            _signatureVerifier = signatureVerifier;
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

        [HttpPost]
        public async Task<IActionResult> ReceiveWebhook([FromBody] WebhookData webhookData)
        {
            // Extract the signature from headers or request body if necessary
            var receivedSignature = Request.Headers["X-PayOS-Signature"].FirstOrDefault();

            if (receivedSignature == null)
            {
                return BadRequest("Signature missing");
            }

            try
            {
                var result = await _paymentService.HandlePaymentSuccess(webhookData);

                if (result)
                {
                    return Ok("Payment processed successfully");
                }
                else
                {
                    return BadRequest("Failed to process payment or invalid signature");
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }
        [HttpPost("payos")]
        public async Task<IActionResult> PayOSWebhook([FromBody] PayOSWebhookPayload payload)
        {
            try
            {
                await _paymentService.ProcessPayOSWebhookAsync(payload, Request.Headers);
                return Ok();
            }
            catch (UnauthorizedAccessException)
            {
                return Unauthorized(new { message = "Invalid webhook payload." });
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = ex.Message });
            }
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
                var result = await _paymentService.ConfirmPaymentAsync(request.TransactionId, request.IsSuccess);
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
    }
}
