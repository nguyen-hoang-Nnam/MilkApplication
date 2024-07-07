using Microsoft.AspNetCore.Http;
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
        /*Task<CreatePaymentResult> HandlePaymentSuccessAsync(PaymentCallBackDTO paymentCallback);*/
        /*Task<PaymentSuccessResponseDTO> HandlePaymentSuccessAsync(PaymentCallBackDTO paymentCallback);
        Task<PaymentConfirmationResponseDTO> ProcessPaymentConfirmationAsync(PaymentConfirmationDTO confirmationDto);*/
        WebhookData VerifyWebhookData(WebhookType webhookType);
        Task<bool> HandlePaymentSuccess(WebhookData webhookData);
        bool VerifyWebhookSignature(WebhookData webhookData, string receivedSignature);
        Task ProcessPayOSWebhookAsync(PayOSWebhookPayload payload, IHeaderDictionary headers);
        Task<ResponseDTO> ConfirmPaymentAsync(string transactionId, bool isSuccess);
    }
}
