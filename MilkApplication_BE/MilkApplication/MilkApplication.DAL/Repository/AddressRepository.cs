using MilkApplication.DAL.Data;
using MilkApplication.DAL.Models;
using MilkApplication.DAL.Repository.IRepositpry;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MilkApplication.DAL.Repository
{
    public class AddressReposiory : GenericRepository<Address>, IAddressRepository
    {
        private readonly AppDbContext _context;
        public AddressReposiory(AppDbContext context) : base(context)
        {
            _context = context;
        }
    }
}
