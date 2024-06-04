using AutoMapper;
using MilkApplication.BLL.Service.IService;
using MilkApplication.DAL.Models;
using MilkApplication.DAL.Models.DTO;
using MilkApplication.DAL.Repository.IRepositpry;
using MilkApplication.DAL.Repository.IRepositpry.UoW;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MilkApplication.BLL.Service
{
    public class ProductService : IProductService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public ProductService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<List<ProductDTO>> GetAllProductsAsync()
        {
            var productGetAll = await _unitOfWork.ProductRepository.GetAllProductsAsync();
            var productMapper = _mapper.Map<List<ProductDTO>>(productGetAll);
            return productMapper;
        }

        public async Task<Product> GetProductByIdAsync(int id)
        {
            var productFound = await _unitOfWork.ProductRepository.GetById(id);
            if (productFound != null)
            {
                return productFound;
            }
            var productMapper = _mapper.Map<Product>(productFound);
            return productMapper;
            
        }

        public async Task<ResponseDTO> AddProductAsync(ProductDTO productDTO)
        {
            var category = await _unitOfWork.CategoryRepository.GetCategoryByIdAsync(productDTO.categoryId);
            if (category == null)
            {
                // Handle scenario where category is not found
                return new ResponseDTO
                {
                    IsSucceed = false,
                    Message = "Category not found",
                };
            }
            var origin = await _unitOfWork.OriginRepository.GetOriginByIdAsync(productDTO.originId);
            if (origin == null)
            {
                // Handle scenario where origin is not found
                return new ResponseDTO
                {
                    IsSucceed = false,
                    Message = "Origin not found",
                };
            }

            var productObj = _mapper.Map<Product>(productDTO);

            productObj.Category = category;
            productObj.Origin = origin;

            await _unitOfWork.ProductRepository.Add(productObj);
            await _unitOfWork.SaveChangeAsync();

            var response = new ResponseDTO
            {
                IsSucceed = true,
                Message = "Product added successfully",
            };
            return response;
        }

        public async Task<ResponseDTO> UpdateProductAsync(int id, ProductDTO productDTO)
        {
            var productUpdate = await _unitOfWork.ProductRepository.GetById(id);
            if (productUpdate != null) {
                productUpdate = _mapper.Map(productDTO, productUpdate);
                await _unitOfWork.ProductRepository.Update(productUpdate);
                var result = await _unitOfWork.SaveChangeAsync();
                if (result > 0)
                {
                    return new ResponseDTO
                    {
                        IsSucceed = true,
                        Message = "Product update successfully!"
                    };
                }
                return new ResponseDTO
                {
                    IsSucceed = false,
                    Message = "Product update failed!"
                };
            }
            return new ResponseDTO
            {
                IsSucceed = false,
                Message = "Product not found!"
            };
        }

        public async Task<ResponseDTO> DeleteProductAsync(int id)
        {
            var deleteProduct = await _unitOfWork.ProductRepository.GetById(id);
            if (deleteProduct != null)
            {
                await _unitOfWork.ProductRepository.Delete(id);
                await _unitOfWork.SaveChangeAsync();

                return new ResponseDTO
                {
                    IsSucceed = true,
                    Message = "Product deleted successfully"
                };
            }
            else
            {
                return new ResponseDTO
                {
                    IsSucceed = false,
                    Message = $"Product with ID {id} not found"
                };
            }
            
        }
    }
}
