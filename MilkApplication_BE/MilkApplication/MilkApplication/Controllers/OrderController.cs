﻿using Microsoft.AspNetCore.Mvc;
using MilkApplication.BLL.Service.IService;
using MilkApplication.DAL.Models.DTO;
using System.Collections.Generic;
using System.Threading.Tasks;
using Newtonsoft.Json;
using MilkApplication.DAL.Helper;
using MilkApplication.DAL.Models.PaginationDTO;
using MilkApplication.DAL.Data;
using Microsoft.EntityFrameworkCore;
using MilkApplication.DAL.enums;

namespace MilkApplication.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private readonly IOrderService _orderService;
        private readonly AppDbContext _context;

        public OrderController(IOrderService orderService, AppDbContext context)
        {
            _orderService = orderService;
            _context = context;
        }

        [HttpGet("GetAllOrder")]
        public async Task<ActionResult<IEnumerable<OrderDTO>>> GetAllOrders()
        {
            var orders = await _orderService.GetAllOrdersAsync();
            return Ok(orders);
        }

        [HttpPost("CreateOrder")]
        public async Task<IActionResult> CreateOrder([FromBody] CreateOrderDTO request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var response = await _orderService.CreateOrderAsync(request);

            if (!response.IsSucceed)
            {
                return BadRequest(response.Message);
            }

            return Ok(response);
        }

        [HttpDelete("DeleteOrder/{orderId}")]
        public async Task<IActionResult> DeleteOrder(int orderId)
        {
            var response = await _orderService.DeleteOrderAsync(orderId);

            if (!response.IsSucceed)
            {
                return NotFound(response.Message);
            }

            return Ok(response);
        }

        [HttpGet]
        public async Task<IActionResult> GetOrderByFilter([FromQuery] PaginationParameter paginationParameter, [FromQuery] OrderFilterDTO orderFilterDTO)
        {
            try
            {
                var result = await _orderService.GetOrderByFilterAsync(paginationParameter, orderFilterDTO);

                var metadata = new
                {
                    result.TotalCount,
                    result.PageSize,
                    result.CurrentPage,
                    result.TotalPages,
                    result.HasNext,
                    result.HasPrevious
                };
                Response.Headers.Add("X-Pagination", JsonConvert.SerializeObject(metadata));
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("completed-payments")]
        public async Task<IActionResult> GetOrdersByCompletedPaymentsAsync()
        {
            try
            {
                var result = await _orderService.GetOrdersByCompletedPaymentsAsync();
                if (result.IsSucceed)
                {
                    return Ok(result.Data);
                }
                else
                {
                    return NotFound(result.Message);
                }
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Internal server error: {ex.Message}");
            }
        }

        [HttpGet("confirm")]
        public async Task<IActionResult> ConfirmPayment(string id, string status, bool cancel)
        {
            if (cancel)
            {
                return BadRequest("Payment was canceled.");
            }

            if (status == "PAID")
            {
                var payment = await _context.Payments.FirstOrDefaultAsync(p => p.TransactionId == id);
                if (payment != null)
                {
                    payment.Status = PaymentStatus.PAID; // Update the payment status to paid
                    await _context.SaveChangesAsync();

                    var order = await _context.Orders.FirstOrDefaultAsync(o => o.orderId == payment.orderId);
                    if (order != null)
                    {
                        order.Status = DAL.enums.OrderStatus.Paid; // Update the order status to paid
                        await _context.SaveChangesAsync();
                    }

                    return Ok("Payment successful.");
                }

                return NotFound("Payment not found.");
            }

            return BadRequest("Payment failed.");
        }
    }
}
