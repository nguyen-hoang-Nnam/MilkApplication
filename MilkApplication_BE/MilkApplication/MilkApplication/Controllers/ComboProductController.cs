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
    public class ComboProductController : ControllerBase
    {
        private readonly IComboProductService _comboProductService;

        public ComboProductController(IComboProductService comboProductService)
        {
            _comboProductService = comboProductService;
        }

        [HttpGet]
        [Route("GetAllComboProducts")]
        public async Task<IActionResult> GetAllComboProduct()
        {

            try
            {
                var result = await _comboProductService.GetAllComboProductsAsync();
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
        [Route("GetComboProductsById/{id:int}")]
        public async Task<ActionResult<Combo>> GetComboProductById(int id)
        {

            try
            {
                var result = await _comboProductService.GetComboProductByIdAsync(id);
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

        [HttpPost]
        [Route("CreateComboProducts")]
        public async Task<IActionResult> CreateComboProduct([FromBody] ComboProductCreateDTO comboProductCreateDTO)
        {
            var result = await _comboProductService.AddComboProductAsync(comboProductCreateDTO);
            if (result.IsSucceed)
            {
                return Ok(result);
            }
            return BadRequest(result);
        }

        [HttpPut]
        [Route("UpdateComboProducts/{id:int}")]
        public async Task<ActionResult> UpdateComboProduct(int id, [FromBody] ComboProductDTO comboProductDTO)
        {
            var response = await _comboProductService.UpdateComboProductAsync(id, comboProductDTO);

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
        [Route("DeleteComboProducts/{id:int}")]
        public async Task<ActionResult> DeleteComboProduct(int id)
        {
            try
            {
                var result = await _comboProductService.DeleteComboProductAsync(id);
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
        public async Task<IActionResult> GetComboProductByFilter([FromQuery] PaginationParameter paginationParameter, [FromQuery] ComboProductFilterDTO comboProductFilterDTO)
        {
            try
            {
                var result = await _comboProductService.GetComboProductByFilterAsync(paginationParameter, comboProductFilterDTO);

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
