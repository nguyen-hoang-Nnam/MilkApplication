using Microsoft.EntityFrameworkCore;
using MilkApplication.DAL.Data;
using MilkApplication.DAL.Models;
using MilkApplication.DAL.Repository.IRepositpry;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MilkApplication.DAL.Repository.IRepositpry.UoW
{
    public interface IUnitOfWork
    {
        // Each repository should be a property in the unit of work
        IProductRepository ProductRepository { get; }
        ICategoryRepository CategoryRepository { get; }
        IOriginRepository OriginRepository { get; }
        ICommentRepository CommentRepository { get; }
        IUserRepository UserRepository { get; }
        IVouchersRepository VouchersRepository { get; }
        ILocationRepository LocationRepository { get; }
        IOrderRepository OrderRepository { get; }
        IOrderItemRepository OrderItemRepository { get; }
        IComboRepository ComboRepository { get; }
        IComboProductRepository ComboProductRepository { get; }
        IPaymentRepository PaymentRepository { get; }
        IAddressRepository AddressRepository { get; }
        AppDbContext dbContext { get; }


        // Save changes to the underlying data store
        public Task<int> SaveChangeAsync();
    }
}
