using AutoMapper;
using Microsoft.AspNetCore.Identity;
using MilkApplication.BLL.Service.IService;
using MilkApplication.DAL.Commons;
using MilkApplication.DAL.enums;
using MilkApplication.DAL.Helper;
using MilkApplication.DAL.Models;
using MilkApplication.DAL.Models.DTO;
using MilkApplication.DAL.Models.PaginationDTO;
using MilkApplication.DAL.Repository.IRepositpry;
using MilkApplication.DAL.Repository.IRepositpry.UoW;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace MilkApplication.BLL.Service
{
    public class ProductService : IProductService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public ProductService(IUnitOfWork unitOfWork, IMapper mapper, UserManager<ApplicationUser> userManager)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _userManager = userManager;
        }

        public async Task<List<ProductDTO>> GetAllProductsAsync()
        {
            var productGetAll = await _unitOfWork.ProductRepository.GetAllAsync();
            var productMapper = _mapper.Map<List<ProductDTO>>(productGetAll);
            return productMapper;
        }

        public async Task<ProductDetailDTO> GetProductByIdAsync(int id)
        {
            var productFound = await _unitOfWork.ProductRepository.GetByIdAsync(id);
            if (productFound == null)
            {
                return null;
            }
            var category = await _unitOfWork.CategoryRepository.GetCategoryByIdAsync(productFound.categoryId);
            var origin = await _unitOfWork.OriginRepository.GetOriginByIdAsync(productFound.originId);
            var location = await _unitOfWork.LocationRepository.GetLocationByIdAsync(productFound.locationId);
            var comment = await _unitOfWork.CommentRepository.GetCommentsByProductIdAsync(productFound.productId);

            var productDetail = new ProductDetailDTO
            {
                productId = productFound.productId,
                productName = productFound.productName,
                Price = productFound.Price,
                discountPrice = productFound.discountPrice,
                discountPercent = productFound.discountPercent,
                productDescription = productFound.productDescription,
                Image = productFound.Image,
                ImagesCarousel = productFound.ImagesCarousel,
                Quantity = productFound.Quantity,
                Status = productFound.Status,
                Category = _mapper.Map<CategoryDTO>(category),
                Origin = _mapper.Map<OriginDTO>(origin),
                Location = _mapper.Map<LocationDTO>(location),
                Comment = _mapper.Map<List<CommentDetailDTO>>(comment)
            };

            return productDetail;

        }

        public async Task<ResponseDTO> AddProductAsync(ProductDTO productDTO)
        {
            var user = await _unitOfWork.UserRepository.GetUserByIdAsync(productDTO.Id);
            if (user == null)
            {
                return new ResponseDTO
                {
                    IsSucceed = false,
                    Message = "User not found",
                };
            }
            var roles = await _userManager.GetRolesAsync(user);
            if (!roles.Contains(UserRole.Admin.ToString()) && !roles.Contains(UserRole.Staff.ToString()))
            {
                return new ResponseDTO
                {
                    IsSucceed = false,
                    Message = "User does not have permission to add products",
                };
            }
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

            productObj.Status = ProductStatus.Valiable;
            productObj.Category = category;
            productObj.Origin = origin;

            await _unitOfWork.ProductRepository.AddAsync(productObj);
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
            try
            {
                var productUpdate = await _unitOfWork.ProductRepository.GetByIdAsync(id);
                if (productUpdate != null)
                {
                    var productId = productUpdate.productId;
                    _mapper.Map(productDTO, productUpdate);
                    productUpdate.productId = productId;
                    await _unitOfWork.ProductRepository.UpdateAsync(productUpdate);
                    var result = await _unitOfWork.SaveChangeAsync();
                    if (result > 0)
                    {
                        return new ResponseDTO
                        {
                            IsSucceed = false,
                            Message = "Product update failed!"
                        };
                    }
                    return new ResponseDTO
                    {
                        
                        IsSucceed = true,
                        Message = "Product update successfully!",
                    };
                }
                return new ResponseDTO
                {
                    IsSucceed = false,
                    Message = "Product not found!"
                };
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}");

                return new ResponseDTO
                {
                    IsSucceed = false,
                    Message = "An error occurred during the product update process."
                };
            }
        }

        public async Task<ResponseDTO> DeleteProductAsync(int id)
        {
            var deleteProduct = await _unitOfWork.ProductRepository.GetByIdAsync(id);
            if (deleteProduct != null)
            {
                await _unitOfWork.ProductRepository.DeleteAsync(id);
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

        public async Task<List<ProductDTO>> GetProductsByCategoryIdAsync(int categoryId)
        {
            var productsByCategory = await _unitOfWork.ProductRepository.GetProductsByCategoryIdAsync(categoryId);
            var productMapper = _mapper.Map<List<ProductDTO>>(productsByCategory);
            return productMapper;
        }
        public async Task<Pagination<ProductDTO>> GetProductByFilterAsync(PaginationParameter paginationParameter, ProductFilterDTO productFilterDTO)
        {
            try
            {
                var products = await _unitOfWork.ProductRepository.GetProductByFilterAsync(paginationParameter, productFilterDTO);
                if (products != null)
                {
                    var mapperResult = _mapper.Map<List<ProductDTO>>(products);
                    return new Pagination<ProductDTO>(mapperResult, products.TotalCount, products.CurrentPage, products.PageSize);
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
