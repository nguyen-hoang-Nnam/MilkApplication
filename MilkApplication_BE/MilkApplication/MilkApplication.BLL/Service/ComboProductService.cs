using AutoMapper;
using MilkApplication.BLL.Service.IService;
using MilkApplication.DAL.Models.DTO;
using MilkApplication.DAL.Models;
using MilkApplication.DAL.Repository.IRepositpry.UoW;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MilkApplication.DAL.enums;
using MilkApplication.DAL.Commons;
using MilkApplication.DAL.Helper;
using MilkApplication.DAL.Models.PaginationDTO;

namespace MilkApplication.BLL.Service
{
    public class ComboProductService : IComboProductService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public ComboProductService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }
        public async Task<ResponseDTO> AddComboProductAsync(ComboProductCreateDTO comboProductCreateDTO)
        {
            var comboExists = await _unitOfWork.ComboRepository.ExistsAsync(comboProductCreateDTO.comboId);
            var productExists = await _unitOfWork.ProductRepository.ExistsAsync(comboProductCreateDTO.productId);

            if (!comboExists || !productExists)
            {
                return new ResponseDTO
                {
                    IsSucceed = false,
                    Message = "Invalid ComboId or ProductId"
                };
            }

            var comboProductObj = _mapper.Map<ComboProduct>(comboProductCreateDTO);
            comboProductObj.Status = ComboStatus.Valiable;
            await _unitOfWork.ComboProductRepository.AddAsync(comboProductObj);
            await _unitOfWork.SaveChangeAsync();

            var response = new ResponseDTO
            {
                IsSucceed = true,
                Message = "Combo Product added successfully",
            };
            return response;
        }

        public async Task<ResponseDTO> DeleteComboProductAsync(int id)
        {
            var deleteComboProduct = await _unitOfWork.ComboProductRepository.GetByIdAsync(id);
            if (deleteComboProduct != null)
            {
                await _unitOfWork.ComboProductRepository.DeleteAsync(id);
                await _unitOfWork.SaveChangeAsync();

                return new ResponseDTO
                {
                    IsSucceed = true,
                    Message = "Combo Product deleted successfully"
                };
            }
            else
            {
                return new ResponseDTO
                {
                    IsSucceed = false,
                    Message = $"Combo Product with ID {id} not found"
                };
            }
        }

        public async Task<List<ComboProductDTO>> GetAllComboProductsAsync()
        {
            var comboProductGetAll = await _unitOfWork.ComboProductRepository.GetAllComboProductAsync();
            var comboProductMapper = _mapper.Map<List<ComboProductDTO>>(comboProductGetAll);
            return comboProductMapper;
        }

        public async Task<ComboProductCreateDTO> GetComboProductByIdAsync(int id)
        {
            var comboProductFound = await _unitOfWork.ComboProductRepository.GetComboProductByIdAsync(id);
            if (comboProductFound == null)
            {
                return null;
            }
            var comboProductMapper = _mapper.Map<ComboProductCreateDTO>(comboProductFound);
            return comboProductMapper;
        }

        public async Task<ResponseDTO> UpdateComboProductAsync(int id, ComboProductDTO comboProductDTO)
        {
            var comboProductUpdate = await _unitOfWork.ComboProductRepository.GetByIdAsync(id);
            if (comboProductUpdate != null)
            {
                comboProductUpdate = _mapper.Map(comboProductDTO, comboProductUpdate);
                await _unitOfWork.ComboProductRepository.UpdateAsync(comboProductUpdate);
                var result = await _unitOfWork.SaveChangeAsync();
                if (result > 0)
                {
                    return new ResponseDTO
                    {
                        IsSucceed = true,
                        Message = "Combo Product update successfully!"
                    };
                }
                return new ResponseDTO
                {
                    IsSucceed = false,
                    Message = "Combo Product update failed!"
                };
            }
            return new ResponseDTO
            {
                IsSucceed = false,
                Message = "Combo Product not found!"
            };
        }
        public async Task<Pagination<ComboProductDTO>> GetComboProductByFilterAsync(PaginationParameter paginationParameter, ComboProductFilterDTO comboProductFilterDTO)
        {
            try
            {
                var comboProducts = await _unitOfWork.ComboProductRepository.GetComboProductByFilterAsync(paginationParameter, comboProductFilterDTO);
                if (comboProducts != null)
                {
                    var mapperResult = _mapper.Map<List<ComboProductDTO>>(comboProducts);
                    return new Pagination<ComboProductDTO>(mapperResult, comboProducts.TotalCount, comboProducts.CurrentPage, comboProducts.PageSize);
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
