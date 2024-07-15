using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using MilkApplication.BLL.Service.IService;
using MilkApplication.DAL.Data;
using MilkApplication.DAL.enums;
using MilkApplication.DAL.Models;
using MilkApplication.DAL.Models.DTO;
using MilkApplication.DAL.Repository.IRepositpry;
using MilkApplication.DAL.Repository.IRepositpry.UoW;
using Net.payOS.Types;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
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
        private readonly IConfiguration _configuration;
        private readonly IUnitOfWork _unitOfWork;

        public PaymentService(IConfiguration configuration, AppDbContext context, Net.payOS.PayOS payOS, IUnitOfWork unitOfWork)
        {
            _clientId = configuration["PayOS:ClientId"];
            _apiKey = configuration["PayOS:ApiKey"];
            _checksumKey = configuration["PayOS:ChecksumKey"];
            _context = context;
            _payOS = payOS;
            _configuration = configuration;
            _unitOfWork = unitOfWork;
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

            foreach (var orderItem in order.OrderDetails)
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

        public WebhookData VerifyWebhookData(WebhookType webhookType)
        {
            // Initialize PayOS instance
            var payOS = new Net.payOS.PayOS(_clientId, _apiKey, _checksumKey);

            // Verify the webhook data
            var verifiedData = payOS.verifyPaymentWebhookData(webhookType);

            return verifiedData;
        }

        public async Task<bool> HandlePaymentSuccess(WebhookData webhookData)
        {
            // Find the payment record using the transaction ID from the webhook data
            var paymentLinkId = webhookData.paymentLinkId;

            if (string.IsNullOrEmpty(paymentLinkId))
            {
                return false;
            }

            // Find the payment record using the transaction ID from the webhook data
            var payment = await _context.Payments.FirstOrDefaultAsync(p => p.TransactionId == paymentLinkId);

            if (payment == null)
            {
                return false;
            }

            // Update the payment status to completed
            payment.Status = PaymentStatus.PAID;
            payment.PaymentDate = DateTime.UtcNow;
            await _context.SaveChangesAsync();

            return true;
        }

        public bool VerifyWebhookSignature(WebhookData webhookData, string receivedSignature)
        {
            var generatedSignature = GenerateSignature(webhookData);

            return generatedSignature == receivedSignature;
        }

        private string GenerateSignature(WebhookData webhookData)
        {
            using (var hmac = new HMACSHA256(Encoding.UTF8.GetBytes(_checksumKey)))
            {
                var dataToSign = $"{webhookData.orderCode}{webhookData.amount}{webhookData.description}{webhookData.paymentLinkId}";
                var hash = hmac.ComputeHash(Encoding.UTF8.GetBytes(dataToSign));
                return BitConverter.ToString(hash).Replace("-", "").ToLower();
            }
        }

        public async Task<ResponseDTO> ConfirmPaymentAsync(string transactionId)
        {
            var response = new ResponseDTO();

            try
            {
                // Check if transactionId is valid
                if (string.IsNullOrWhiteSpace(transactionId))
                {
                    response.Message = "Transaction ID cannot be null or empty.";
                    return response;
                }

                // Check if unitOfWork is null
                if (_unitOfWork == null)
                {
                    response.Message = "UnitOfWork is not initialized.";
                    return response;
                }

                // Log the state of unitOfWork and its repositories
                Console.WriteLine($"_unitOfWork: {_unitOfWork}");
                Console.WriteLine($"PaymentRepository: {_unitOfWork.PaymentRepository}");
                Console.WriteLine($"OrderRepository: {_unitOfWork.OrderRepository}");

                // Retrieve payment
                var payment = await _unitOfWork.PaymentRepository.GetByTransactionIdAsync(transactionId);
                if (payment == null)
                {
                    response.Message = "Payment not found.";
                    return response;
                }

                bool isSuccess = true;

                // Update payment status
                payment.Status = isSuccess ? PaymentStatus.PAID : PaymentStatus.Failed;

                // Check if PaymentRepository is null
                if (_unitOfWork.PaymentRepository == null)
                {
                    response.Message = "PaymentRepository is not initialized.";
                    return response;
                }

                await _unitOfWork.PaymentRepository.UpdatePaymentAsync(payment);

                // Retrieve and update order
                var order = await _unitOfWork.OrderRepository.GetByIdAsync(payment.orderId);
                if (order != null)
                {
                    order.Status = isSuccess ? OrderStatus.Paid : OrderStatus.Unpaid;

                    // Check if OrderRepository is null
                    if (_unitOfWork.OrderRepository == null)
                    {
                        response.Message = "OrderRepository is not initialized.";
                        return response;
                    }

                    await _unitOfWork.OrderRepository.UpdateOrderAsync(order);
                }

                await _unitOfWork.SaveChangeAsync();

                response.IsSucceed = true;
                response.Message = "Payment confirmed successfully.";
            }
            catch (Exception ex)
            {
                response.Message = $"Failed to confirm payment: {ex.Message}";
            }

            return response;
        }

        public async Task<decimal> GetTotalAmountByDayAsync(DateTime date)
        {
            return await _unitOfWork.PaymentRepository.SumAsync(
                p => p.PaymentDate.Date == date.Date,
                p => p.Amount
            );
        }

        public async Task<decimal> GetTotalAmountByMonthAsync(int year, int month)
        {
            return await _unitOfWork.PaymentRepository.SumAsync(
                p => p.PaymentDate.Year == year && p.PaymentDate.Month == month,
                p => p.Amount
            );
        }

        public async Task<Dictionary<string, decimal>> GetTotalAmountsForLast12MonthsAsync()
        {
            return await _unitOfWork.PaymentRepository.GetMonthlyTotalsAsync(12);
        }

    }
}
