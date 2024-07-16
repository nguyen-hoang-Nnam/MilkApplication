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
        private readonly AppDbContext _milkDBContext;
        private readonly IProductRepository _productRepository;
        private readonly ICategoryRepository _categoryRepository;
        private readonly IOriginRepository _originRepository;
        private readonly ICommentRepository _commentRepository;
        private readonly IUserRepository _userRepository;
        private readonly IVouchersRepository _vouchersRepository;
        private readonly ILocationRepository _locationRepository;
        private readonly IOrderRepository _orderRepository;
        private readonly IOrderItemRepository _orderItemRepository;
        private readonly IComboRepository _comboRepository;
        private readonly IComboProductRepository _comboProductRepository;
        private readonly IPaymentRepository _paymentRepository;
        private readonly IAddressRepository _addressRepository;
        private readonly AppDbContext _dbContext;


        private bool disposed = false;

        public UnitOfWork(AppDbContext milkDBContext, IProductRepository productRepository, AppDbContext dbContext, ICategoryRepository categoryRepository, IOriginRepository originRepository, ICommentRepository commentRepository, IUserRepository userRepository, IVouchersRepository vouchersRepository, ILocationRepository locationRepository, IOrderRepository orderRepository, IOrderItemRepository orderItemRepository, IComboRepository comboRepository, IComboProductRepository comboProductRepository, IPaymentRepository paymentRepository, IAddressRepository addressRepository)
        {
            this._milkDBContext = milkDBContext;
            this._productRepository = productRepository;
            this._dbContext = dbContext;
            this._categoryRepository = categoryRepository;
            this._originRepository = originRepository;
            this._commentRepository = commentRepository;
            this._userRepository = userRepository;
            this._vouchersRepository = vouchersRepository;
            this._locationRepository = locationRepository;
            this._orderRepository = orderRepository;
            this._orderItemRepository = orderItemRepository;
            this._comboRepository = comboRepository;
            this._comboProductRepository = comboProductRepository;
            this._paymentRepository = paymentRepository;
            this._addressRepository = addressRepository;
            _paymentRepository = paymentRepository;
        }

        public IProductRepository ProductRepository { get { return _productRepository; } }
        public ICategoryRepository CategoryRepository { get { return _categoryRepository; } }
        public IOriginRepository OriginRepository { get { return _originRepository; } }
        public ICommentRepository CommentRepository { get { return _commentRepository; } }
        public IUserRepository UserRepository { get { return _userRepository; } }
        public IVouchersRepository VouchersRepository { get { return _vouchersRepository; } }
        public ILocationRepository LocationRepository { get { return _locationRepository; } }
        public IOrderRepository OrderRepository { get { return _orderRepository; } }
        public IOrderItemRepository OrderItemRepository { get { return _orderItemRepository; } }
        public IComboRepository ComboRepository { get { return _comboRepository; } }
        public IComboProductRepository ComboProductRepository { get { return _comboProductRepository; } }
        public IPaymentRepository PaymentRepository { get { return _paymentRepository; } }
        public IAddressRepository AddressRepository { get { return _addressRepository; } }  
        public AppDbContext dbContext { get { return _dbContext; } }

        public async Task<int> SaveChangeAsync()
        {
            return await _dbContext.SaveChangesAsync();

        }

        protected virtual void Dispose(bool disposing)
        {
            if (!disposed)
            {
                if (disposing)
                {
                    _dbContext.Dispose();
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
