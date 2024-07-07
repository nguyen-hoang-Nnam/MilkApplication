using Microsoft.AspNetCore.Mvc;
using MilkApplication.BLL.Service.IService;
using MilkApplication.DAL.Models.DTO;
using System.Collections.Generic;
using System.Threading.Tasks;
using Newtonsoft.Json;
using MilkApplication.DAL.Helper;
using MilkApplication.DAL.Models.PaginationDTO;

namespace MilkApplication.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private readonly IOrderService _orderService;

        public OrderController(IOrderService orderService)
        {
            _orderService = orderService;
        }

        [HttpGet("GetAllOrder")]
        public async Task<ActionResult<IEnumerable<OrderDTO>>> GetAllOrders()
        {
            var orders = await _orderService.GetAllOrdersAsync();
            return Ok(orders);
        }

        [HttpPost("CreateOrder")]
        public async Task<IActionResult> CreateOrder([FromBody] OrderRequestDTO request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var response = await _orderService.CreateOrderAsync(request.Id, request.OrderItemDTOs, request.VoucherCode);

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
    }
}
