using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MilkApplication.DAL.Models.PaginationDTO
{
    public class CommentFilterDTO
    {
        public string Sort { get; set; } = "id";
        public string SortDirection { get; set; } = "desc";
        public string? Search { get; set; }
        public int? productId { get; set; }
    }
}
