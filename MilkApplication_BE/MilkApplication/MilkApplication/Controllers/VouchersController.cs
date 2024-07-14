using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MilkApplication.BLL.Service.IService;
using MilkApplication.DAL.Models.DTO;
using MilkApplication.DAL.Models;
using Microsoft.AspNetCore.Http.HttpResults;

namespace MilkApplication.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VouchersController : ControllerBase
    {
        private readonly IVouchersService _voucherService;

        public VouchersController(IVouchersService voucherService)
        {
            _voucherService = voucherService;
        }

        [HttpGet]
        [Route("GetAllVouchers")]
        public async Task<IActionResult> GetAllVouchers()
        {

            try
            {
                var result = await _voucherService.GetAllVouchersAsync();
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
        [Route("GetVouchersById/{id:int}")]
        public async Task<ActionResult<Vouchers>> GetVoucherById(int id)
        {

            try
            {
                var result = await _voucherService.GetVouchersByIdAsync(id);
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
        [Route("CreateVouchers")]
        public async Task<IActionResult> CreateCategory([FromBody] VouchersDTO vouchersDTO)
        {
            var result = await _voucherService.AddVouchersAsync(vouchersDTO);
            if (result.IsSucceed)
            {
                return Ok(result);
            }
            return BadRequest(result);
        }

        [HttpPut]
        [Route("UpdateVouchers/{id:int}")]
        public async Task<ActionResult> UpdateVoucher(int id, [FromBody] VouchersDTO vouchersDTO)
        {
            var response = await _voucherService.UpdateVouchersAsync(id, vouchersDTO);

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
        [Route("DeleteVouchers/{id:int}")]
        public async Task<ActionResult> DeleteVoucher(int id)
        {
            try
            {
                var result = await _voucherService.DeleteVouchersAsync(id);
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

        [HttpGet("GetActiveVouchers")]
        public async Task<IActionResult> GetActiveVouchers()
        {
            try
            {
                var result = await _voucherService.GetActiveVouchersAsync();
                if (result != null)
                {
                    return Ok(result);
                }
                return NotFound(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
