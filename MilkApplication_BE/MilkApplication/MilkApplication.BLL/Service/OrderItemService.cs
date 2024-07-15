using AutoMapper;
using MilkApplication.BLL.Service.IService;
using MilkApplication.DAL.Commons;
using MilkApplication.DAL.Helper;
using MilkApplication.DAL.Models;
using MilkApplication.DAL.Models.DTO;
using MilkApplication.DAL.Models.PaginationDTO;
using MilkApplication.DAL.Repository.IRepositpry.UoW;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MilkApplication.BLL.Service
{
    public class OrderItemService : IOrderItemService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public OrderItemService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }
        public async Task<Pagination<OrderDetailDTO>> GetOrderItemByFilterAsync(PaginationParameter paginationParameter, OrderItemFilterDTO orderItemFilterDTO)
        {
            try
            {
                var orderitems = await _unitOfWork.OrderItemRepository.GetOrderItemByFilterAsync(paginationParameter, orderItemFilterDTO);
                if (orderitems != null)
                {
                    var mapperResult = _mapper.Map<List<OrderDetailDTO>>(orderitems);
                    return new Pagination<OrderDetailDTO>(mapperResult, orderitems.TotalCount, orderitems.CurrentPage, orderitems.PageSize);
                }
                return null;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
