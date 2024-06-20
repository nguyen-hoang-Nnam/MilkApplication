using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MilkApplication.BLL.Service;
using MilkApplication.BLL.Service.IService;
using MilkApplication.DAL.Models;

namespace MilkApplication.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderItemController : ControllerBase
    {
        private readonly IOrderItemService _orderItemService;

        public OrderItemController(IOrderItemService orderItemService)
        {
            _orderItemService = orderItemService;
        }
    }
}
