using AutoMapper;
using MilkApplication.BLL.Service.IService;
using MilkApplication.DAL.Models;
using MilkApplication.DAL.Models.DTO;
using MilkApplication.DAL.Repository.IRepositpry.UoW;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MilkApplication.BLL.Service
{
    public class CategoryService : ICategoryService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public CategoryService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }
        public async Task<ResponseDTO> AddCategoryAsync(CategoryDTO categoryDTO)
        {
            var categoryObj = _mapper.Map<Category>(categoryDTO);
            await _unitOfWork.CategoryRepository.AddAsync(categoryObj);
            await _unitOfWork.SaveChangeAsync();

            var response = new ResponseDTO
            {
                IsSucceed = true,
                Message = "Category added successfully",
            };
            return response;
        }

        public async Task<ResponseDTO> DeleteCategoryAsync(int id)
        {
            var deleteCategory = await _unitOfWork.CategoryRepository.GetByIdAsync(id);
            if (deleteCategory != null)
            {
                await _unitOfWork.CategoryRepository.DeleteAsync(id);
                await _unitOfWork.SaveChangeAsync();

                return new ResponseDTO
                {
                    IsSucceed = true,
                    Message = "Category deleted successfully"
                };
            }
            else
            {
                return new ResponseDTO
                {
                    IsSucceed = false,
                    Message = $"Category with ID {id} not found"
                };
            }
        }

        public async Task<List<CategoryDTO>> GetAllCategorysAsync()
        {
            var categoryGetAll = await _unitOfWork.CategoryRepository.GetAllAsync();
            var categoryMapper = _mapper.Map<List<CategoryDTO>>(categoryGetAll);
            return categoryMapper;
        }

        public async Task<CategoryDTO> GetCategoryByIdAsync(int id)
        {
            var categoryFound = await _unitOfWork.CategoryRepository.GetByIdAsync(id);
            if (categoryFound == null)
            {
                return null;
            }
            var categoryMapper = _mapper.Map<CategoryDTO>(categoryFound);
            return categoryMapper;
        }

        public async Task<ResponseDTO> UpdateCategoryAsync(int id, CategoryDTO categoryDTO)
        {
            var categoryUpdate = await _unitOfWork.CategoryRepository.GetByIdAsync(id);
            if (categoryUpdate != null)
            {
                categoryUpdate = _mapper.Map(categoryDTO, categoryUpdate);
                await _unitOfWork.CategoryRepository.UpdateAsync(categoryUpdate);
                var result = await _unitOfWork.SaveChangeAsync();
                if (result > 0)
                {
                    return new ResponseDTO
                    {
                        IsSucceed = true,
                        Message = "Category update successfully!"
                    };
                }
                return new ResponseDTO
                {
                    IsSucceed = false,
                    Message = "Category update failed!"
                };
            }
            return new ResponseDTO
            {
                IsSucceed = false,
                Message = "Category not found!"
            };
        }
    }
}
