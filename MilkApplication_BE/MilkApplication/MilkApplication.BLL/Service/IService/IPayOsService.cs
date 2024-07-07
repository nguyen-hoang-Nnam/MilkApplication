using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MilkApplication.BLL.Service.IService
{
    public interface IPayOsService
    {
        Task<string> GetPaymentStatus(string transactionId);
    }
}
