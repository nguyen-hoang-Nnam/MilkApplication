using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using MilkApplication.BLL.Service;
using MilkApplication.BLL.Service.IService;
using MilkApplication.DAL.Helper;
using MilkApplication.DAL.Models;
using MilkApplication.DAL.Models.DTO;
using MilkApplication.DAL.Models.PaginationDTO;
using MilkApplication.DAL.Repository.IRepositpry;
using Newtonsoft.Json;
using System.Security.Claims;

namespace MilkApplication.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AddressController : ControllerBase
    {
        private readonly IAddressService _addressService;
        private readonly IMapper _mapper;
        public AddressController(IAddressService addressService, IMapper mapper)
        {
            _addressService = addressService;
            _mapper = mapper;
        }
        [HttpPost]
        [Route("CreateAddress")]
        public async Task<IActionResult> CreateAddress([FromBody] AddressDTO addressDTO)
        {
            var result = await _addressService.AddAddressAsync(addressDTO);
            if (result.IsSucceed)
            {
                return Ok(result);
            }
            return BadRequest(result);
        }
        [HttpGet]
        [Route("GetAllAddress")]
        public async Task<IActionResult> GetAllComment()
        {
            var comments = await _addressService.GetAllAddressAsync();
            return Ok(comments);
        }
        [HttpPut]
        [Route("UpdateAddress/{id}")]
        public async Task<IActionResult> UpdateComment(int id, [FromBody] AddressDTO address)
        {
            var response = await _addressService.UpdateAddressAsync(id, address);

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
        [Route("DeleteAddress/{id}")]
        public async Task<IActionResult> DeleteAddress(int id)
        {
            try
            {
                var result = await _addressService.DeleteAddressAsync(id);
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