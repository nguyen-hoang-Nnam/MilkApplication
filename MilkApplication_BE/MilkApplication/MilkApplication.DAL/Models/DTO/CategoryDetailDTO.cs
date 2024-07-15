﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MilkApplication.DAL.Models.DTO
{
    public class CategoryDetailDTO
    {
        public int categoryId { get; set; }
        public string categoryName { get; set; }
        public List<ProductDetailCategoryDTO> Product { get; set; }
    }
}
