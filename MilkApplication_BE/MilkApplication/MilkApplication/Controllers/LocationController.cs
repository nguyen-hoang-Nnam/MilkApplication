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
    public class LocationController : ControllerBase
    {
        private readonly ILocationService _locationService;

        public LocationController(ILocationService locationService)
        {
            _locationService = locationService;
        }

        [HttpGet]
        [Route("GetAllLocation")]
        public async Task<IActionResult> GetAllLocation()
        {

            try
            {
                var result = await _locationService.GetAllLocationsAsync();
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
        [Route("GetLocationsById/{id:int}")]
        public async Task<ActionResult<Location>> GetLocationById(int id)
        {

            try
            {
                var result = await _locationService.GetLocationByIdAsync(id);
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
        [Route("CreateLocation")]
        public async Task<IActionResult> CreateLocation([FromBody] LocationDTO locationDTO)
        {
            var result = await _locationService.AddLocationAsync(locationDTO);
            if (result.IsSucceed)
            {
                return Ok(result);
            }
            return BadRequest(result);
        }

        [HttpPut]
        [Route("UpdateLocation/{id:int}")]
        public async Task<ActionResult> UpdateOrigin(int id, [FromBody] LocationDTO locationDTO)
        {
            var response = await _locationService.UpdateLocationAsync(id, locationDTO);

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
        [Route("DeleteLocation/{id:int}")]
        public async Task<ActionResult> DeleteLocation(int id)
        {
            try
            {
                var result = await _locationService.DeleteLocationAsync(id);
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
        public async Task<IActionResult> GetLocationByFilter([FromQuery] PaginationParameter paginationParameter, [FromQuery] LocationFilterDTO locationFilterDTO)
        {
            try
            {
                var result = await _locationService.GetLocationByFilterAsync(paginationParameter, locationFilterDTO);

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
