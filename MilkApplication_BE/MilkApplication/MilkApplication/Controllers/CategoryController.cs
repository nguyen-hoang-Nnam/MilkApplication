using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MilkApplication.BLL.Service.IService;
using MilkApplication.DAL.Models.DTO;
using MilkApplication.DAL.Models;
using MilkApplication.BLL.Service;
using MilkApplication.DAL.Helper;
using MilkApplication.DAL.Models.PaginationDTO;
using Newtonsoft.Json;

namespace MilkApplication.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        private readonly ICategoryService _categoryService;

        public CategoryController(ICategoryService categoryService, IProductService productService)
        {
            _categoryService = categoryService;
        }

        [HttpGet]
        [Route("GetAllCategorys")]
        public async Task<IActionResult> GetAllCategory()
        {

            try
            {
                var result = await _categoryService.GetAllCategorysAsync();
                if (result != null)
                {
                    return Ok(result);
                }
                return NotFound(result);
            }
            catch (Exception ex)
            {

                return BadRequest(ex);
            }
        }

        [HttpGet]
        [Route("GetCategorysById")]
        public async Task<ActionResult<Category>> GetCategoryById(int id)
        {

            try
            {
                var result = await _categoryService.GetCategoryByIdAsync(id);
                if (result != null)
                {
                    return Ok(result);
                }
                return NotFound();
            }
            catch (Exception ex)
            {

                return BadRequest(ex.Message);
            }
        }
        [HttpGet("GetProductInCategory")]
        public async Task<IActionResult> GetAllCategoriesWithProducts()
        {
            var categoryDetail = await _categoryService.GetAllCategoriesWithProductsAsync();
            if (categoryDetail == null)
            {
                return NotFound();
            }
            return Ok(categoryDetail);
        }

        [HttpPost]
        [Route("CreateCategorys")]
        public async Task<IActionResult> CreateCategory([FromBody] CategoryDTO categoryDTO)
        {
            var result = await _categoryService.AddCategoryAsync(categoryDTO);
            if (result.IsSucceed)
            {
                return Ok(result);
            }
            return BadRequest(result);
        }

        [HttpPut]
        [Route("UpdateCategorys/{id:int}")]
        public async Task<ActionResult> UpdateCategory(int id, [FromBody] CategoryDTO categoryDTO)
        {
            var response = await _categoryService.UpdateCategoryAsync(id, categoryDTO);

            if (response.IsSucceed)
            {
                return Ok(response);
            }
            else
            {
                return NotFound(response);
            }
        }

        [HttpDelete]
        [Route("DeleteCategorys/{id:int}")]
        public async Task<ActionResult> DeleteCategory(int id)
        {
            try
            {
                var result = await _categoryService.DeleteCategoryAsync(id);
                if (result.IsSucceed == true)
                {
                    return Ok(result);
                }
                return NotFound();
            }
            catch (Exception ex)
            {

                return BadRequest(ex.Message);
            }
        }
        [HttpGet]
        public async Task<IActionResult> GetCategoryByFilter([FromQuery] PaginationParameter paginationParameter, [FromQuery] CategoryFilterDTO categoryFilterDTO)
        {
            try
            {
                var result = await _categoryService.GetCategoryByFilterAsync(paginationParameter, categoryFilterDTO);

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
    }
}
