using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MilkApplication.BLL.Service.IService;
using MilkApplication.DAL.Models.DTO;
using MilkApplication.DAL.Models;

namespace MilkApplication.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ComboController : ControllerBase
    {
        private readonly IComboService _comboService;

        public ComboController(IComboService comboService)
        {
            _comboService = comboService;
        }

        [HttpGet]
        [Route("GetAllCombos")]
        public async Task<IActionResult> GetAllCombo()
        {

            try
            {
                var result = await _comboService.GetAllCombosAsync();
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
        [Route("GetCombosById/{id:int}")]
        public async Task<ActionResult<Combo>> GetComboById(int id)
        {

            try
            {
                var result = await _comboService.GetComboByIdAsync(id);
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
        [Route("CreateCombos")]
        public async Task<IActionResult> CreateCombo([FromBody] ComboDTO comboDTO)
        {
            var result = await _comboService.AddComboAsync(comboDTO);
            if (result.IsSucceed)
            {
                return Ok(result);
            }
            return BadRequest(result);
        }

        [HttpPut]
        [Route("UpdateCombos/{id:int}")]
        public async Task<ActionResult> UpdateCombo(int id, [FromBody] ComboDTO comboDTO)
        {
            var response = await _comboService.UpdateComboAsync(id, comboDTO);

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
        [Route("DeleteCombos/{id:int}")]
        public async Task<ActionResult> DeleteCombo(int id)
        {
            try
            {
                var result = await _comboService.DeleteComboAsync(id);
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
    }
}
