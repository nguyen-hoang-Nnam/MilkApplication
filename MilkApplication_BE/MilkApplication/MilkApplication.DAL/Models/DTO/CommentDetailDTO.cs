using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MilkApplication.DAL.Models.DTO
{
    public class CommentDetailDTO
    {
        public List<CommentUserDetailDTO> User{ get; set; }
        public int commentId { get; set; }
        public string commentDetail { get; set; } 
        public int Rating { get; set; }
        public DateTime Date { get; set; } = DateTime.UtcNow.AddHours(7);
    }
}
