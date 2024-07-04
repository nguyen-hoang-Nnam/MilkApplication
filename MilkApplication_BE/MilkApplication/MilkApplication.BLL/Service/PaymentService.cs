using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using MilkApplication.BLL.Service.IService;
using MilkApplication.DAL.Data;
using MilkApplication.DAL.enums;
using MilkApplication.DAL.Models;
using MilkApplication.DAL.Models.DTO;
using Net.payOS.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AppCreatePaymentResult = MilkApplication.DAL.Models.DTO.CreatePaymentResult;

namespace MilkApplication.BLL.Service
{
    public class PaymentService : IPaymentService
    {
        private readonly string _clientId;
        private readonly string _apiKey;
        private readonly string _checksumKey;
        private readonly Net.payOS.PayOS _payOS;
        private readonly AppDbContext _context;

        public PaymentService(IConfiguration configuration, AppDbContext context, Net.payOS.PayOS payOS)
        {
            _clientId = configuration["PayOS:ClientId"];
            _apiKey = configuration["PayOS:ApiKey"];
            _checksumKey = configuration["PayOS:ChecksumKey"];
            _context = context;
            _payOS = payOS;
        }
        public async Task<AppCreatePaymentResult> CreatePaymentLink(Order order, string cancelUrl, string returnUrl)
        {
            var existingPayment = await _context.Payments
                    .FirstOrDefaultAsync(p => p.orderId == order.orderId && p.Status == PaymentStatus.Pending);

            if (existingPayment != null)
            {
                // If payment exists, return the existing payment link
                return new AppCreatePaymentResult
                {
                    PaymentUrl = existingPayment.PaymentUrl,
                    TransactionId = existingPayment.TransactionId,
                    Success = true
                };
            }
            var items = new List<ItemData>();

            foreach (var orderItem in order.OrderItems)
            {
                var item = new ItemData(orderItem.Product?.productName ?? "Unknown Product", orderItem.Quantity, (int)orderItem.Price);
                items.Add(item);
            }

            var paymentData = new PaymentData(order.orderId, (int)order.totalPrice, "Thanh toan don hang", items, cancelUrl, returnUrl);
            Net.payOS.Types.CreatePaymentResult paymentResult = await _payOS.createPaymentLink(paymentData);

            if (paymentResult != null && !string.IsNullOrEmpty(paymentResult.checkoutUrl))
            {
                var payment = new Payment
                {
                    orderId = order.orderId,
                    PaymentDate = DateTime.Now,
                    Amount = order.totalPrice,
                    Status = PaymentStatus.Pending,
                    paymentMethodId = 1, 
                    TransactionId = paymentResult.paymentLinkId,
                    PaymentUrl = paymentResult.checkoutUrl
                };

                _context.Payments.Add(payment);
                await _context.SaveChangesAsync();

                return new AppCreatePaymentResult
                {
                    PaymentUrl = paymentResult.checkoutUrl,
                    TransactionId = paymentResult.paymentLinkId,
                    Success = true
                };
            }

            return new AppCreatePaymentResult
            {
                Success = false,
                ErrorMessage = "Failed to create payment link"
            };
        }
    }
}
