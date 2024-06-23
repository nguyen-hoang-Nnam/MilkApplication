using MilkApplication.DAL.Models;
using MilkApplication.DAL.Models.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MilkApplication.DAL.Repository.IRepositpry
{
    public interface IComboProductRepository : IGenericRepository<ComboProduct>
    {
        Task<IEnumerable<ComboProduct>> GetAllComboProductAsync();
        Task<ComboProduct> GetComboProductByIdAsync(int id);
    }
}
