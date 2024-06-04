using MilkApplication.DAL.Data;
using MilkApplication.DAL.Models;
using MilkApplication.DAL.Repository.IRepositpry;
using MilkApplication.DAL.Repository.IRepositpry.UoW;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MilkApplication.DAL.Repository
{
    public class UnitOfWork : IUnitOfWork, IDisposable
    {
        private readonly AppDbContext _context;
        private readonly IProductRepository _productRepository;
        private readonly ICategoryRepository _categoryRepository;
        private readonly IOriginRepository _originRepository;
        private readonly AppDbContext _dbContext;


        private bool disposed = false;

        public UnitOfWork(AppDbContext context, IProductRepository productRepository, AppDbContext dbContext, ICategoryRepository categoryRepository, IOriginRepository originRepository)
        {
            _context = context;
            _productRepository = productRepository;
            _dbContext = dbContext;
            _categoryRepository = categoryRepository;
            _originRepository = originRepository;
        }

        public IProductRepository ProductRepository { get { return _productRepository; } }
        public ICategoryRepository CategoryRepository { get { return _categoryRepository; } }
        public IOriginRepository OriginRepository { get { return _originRepository; } }
        public AppDbContext dbContext { get { return _dbContext; } }

        public async Task<int> SaveChangeAsync()
        {
            return await _context.SaveChangesAsync();

        }

        protected virtual void Dispose(bool disposing)
        {
            if (!disposed)
            {
                if (disposing)
                {
                    _context.Dispose();
                }
                disposed = true;
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }


    }
}
