using MilkApplication.BLL.Service.IService;
using MilkApplication.DAL.Models;
using MilkApplication.DAL.Repository.IRepositpry;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MilkApplication.BLL.Service
{
    public class ProductService : IProductService
    {
        private readonly IGenericRepository<Product> _productRepositpory;

        public ProductService(IGenericRepository<Product> productRepositpory)
        {
            _productRepositpory = productRepositpory;
        }

        public async Task<IEnumerable<Product>> GetAllProducts()
        {
            return await _productRepositpory.GetAll();
        }

        public async Task<Product> GetProductById(int id)
        {
            return await _productRepositpory.GetById(id);
        }

        public async Task AddProduct(Product product)
        {
            await _productRepositpory.Add(product);
        }

        public async Task UpdateProduct(Product product)
        {
            await _productRepositpory.Update(product);
        }

        public async Task DeleteProduct(int id)
        {
            await _productRepositpory.Delete(id);
        }
    }
}
