using MilkApplication.DAL.enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MilkApplication.DAL.Models.PaginationDTO
{
    public class ProductFilterDTO
    {
        public string Sort { get; set; } = "id";
        public string SortDirection { get; set; } = "desc";
        public ProductStatus Status { get; set; }
        public string? Search { get; set; }
        public int? CategoryId { get; set; }
        public int? OriginId { get; set; }
        public int? LocationId { get; set; }
    }
}
