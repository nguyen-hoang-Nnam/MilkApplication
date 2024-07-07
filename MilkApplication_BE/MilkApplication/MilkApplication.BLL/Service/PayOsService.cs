using MilkApplication.BLL.Service.IService;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MilkApplication.BLL.Service
{
    public class PayOsService : IPayOsService
    {
        private readonly HttpClient _httpClient;

        public PayOsService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<string> GetPaymentStatus(string transactionId)
        {
            var response = await _httpClient.GetAsync($"https://api.payos.com/payment/status/{transactionId}");

            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                var paymentStatusResponse = JsonConvert.DeserializeObject<PaymentStatusResponse>(content);
                return paymentStatusResponse.Status;
            }

            return null;
        }

        private class PaymentStatusResponse
        {
            public string Status { get; set; }
        }
    }
}
