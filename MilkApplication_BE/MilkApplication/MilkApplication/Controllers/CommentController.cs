using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
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
    public class CommentController : ControllerBase
    {
        private readonly ICommentService _commentService;
        private readonly IMapper _mapper;
        public CommentController(ICommentService commentService, IMapper mapper) 
        {
            _commentService = commentService;
            _mapper = mapper;
        }
        [HttpGet]
        [Route("GetAllComment")]
        public async Task<IActionResult> GetAllComment()
        {
            var comments = await _commentService.GetAllCommentAsync();
            return Ok(comments);
        }
        [HttpPost]
        [Route("CreateComment")]
        public async Task<IActionResult> CreateComment([FromBody] CommentDTO commentDTO)
        {
            var result = await _commentService.AddCommentAsync(commentDTO);
            if (result.IsSucceed)
            {
                return Ok(result);
            }
            return BadRequest(result);
        }
        [HttpPut]
        [Route("UpdateComment/{id}")]
        public async Task<IActionResult> UpdateComment(int id, [FromBody] CommentDTO comment)
        {
            var commentToUpdate = await _commentService.GetCommentByIdAsync(id);
            if (commentToUpdate == null)
            {
                return BadRequest("Comment not found");
            }
            await _commentService.UpdateCommentAsync(id, comment);
            return Ok();
        }
        [HttpDelete]
        [Route("DeleteComment/{id}")]
        public async Task<IActionResult> DeleteComment(int id)
        {
            try
            {
                var result = await _commentService.DeleteCommentAsync(id);
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
        [HttpGet("filter")]
        public async Task<IActionResult> GetCommentByFilter([FromQuery] PaginationParameter paginationParameter, [FromQuery] CommentFilterDTO commentFilterDTO)
        {
            try
            {
                var result = await _commentService.GetCommentByFilterAsync(paginationParameter, commentFilterDTO);

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
