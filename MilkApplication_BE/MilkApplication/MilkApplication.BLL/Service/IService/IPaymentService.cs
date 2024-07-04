using MilkApplication.DAL.Models;
using MilkApplication.DAL.Models.DTO;
using Net.payOS.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CreatePaymentResult = MilkApplication.DAL.Models.DTO.CreatePaymentResult;


namespace MilkApplication.BLL.Service.IService
{
    public interface IPaymentService
    {
        Task<CreatePaymentResult> CreatePaymentLink(Order order, string cancelUrl, string returnUrl);
    }
}
