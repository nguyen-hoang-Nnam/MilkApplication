using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using MilkApplication.BLL.Service.IService;
using MilkApplication.DAL.Models;
using MilkApplication.DAL.Models.DTO;
using MilkApplication.DAL.Repository.IRepositpry;
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
        public async Task<IActionResult> GetAllComment()
        {
            var comments = await _commentService.GetAllCommentAsync();
            return Ok(comments);
        }
        [HttpPost]
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
        [HttpDelete("{id}")]
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
    }
}
