﻿using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MilkApplication.BLL.Service.IService;
using MilkApplication.DAL.Models.DTO;
using MilkApplication.DAL.Models;

namespace MilkApplication.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OriginController : ControllerBase
    {
        private readonly IOriginService _originService;

        public OriginController(IOriginService originService)
        {
            _originService = originService;
        }

        [HttpGet]
        [Route("GetAllOrigins")]
        public async Task<IActionResult> GetAllOrigin()
        {

            try
            {
                var result = await _originService.GetAllOriginsAsync();
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
        [Route("GetOriginsById/{id:int}")]
        public async Task<ActionResult<Origin>> GetOriginById(int id)
        {

            try
            {
                var result = await _originService.GetOriginByIdAsync(id);
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
        [Route("CreateOrigins")]
        public async Task<IActionResult> CreateOrigin([FromBody] OriginDTO originDTO)
        {
            var result = await _originService.AddOriginAsync(originDTO);
            if (result.IsSucceed)
            {
                return Ok(result);
            }
            return BadRequest(result);
        }

        [HttpPut]
        [Route("UpdateOrigins/{id:int}")]
        public async Task<ActionResult> UpdateOrigin(int id, [FromBody] OriginDTO originDTO)
        {
            var response = await _originService.UpdateOriginAsync(id, originDTO);

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
        [Route("DeleteOrigins/{id:int}")]
        public async Task<ActionResult> DeleteOrigin(int id)
        {
            try
            {
                var result = await _originService.DeleteOriginAsync(id);
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
