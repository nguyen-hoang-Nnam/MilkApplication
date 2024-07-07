using Net.payOS.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace MilkApplication.BLL.Service
{
    public class SignatureVerifier
    {
        private readonly string _checksumKey;

        public SignatureVerifier(string checksumKey)
        {
            _checksumKey = checksumKey;
        }

        public bool VerifySignature(WebhookData webhookData, string receivedSignature)
        {
            string dataToSign = $"{webhookData.orderCode}{webhookData.amount}{webhookData.description}{webhookData.paymentLinkId}";
            string generatedSignature = GenerateSignature(dataToSign);

            return generatedSignature.Equals(receivedSignature, StringComparison.OrdinalIgnoreCase);
        }

        private string GenerateSignature(string data)
        {
            using (var hmac = new HMACSHA256(Encoding.UTF8.GetBytes(_checksumKey)))
            {
                byte[] hash = hmac.ComputeHash(Encoding.UTF8.GetBytes(data));
                return BitConverter.ToString(hash).Replace("-", "").ToLower();
            }
        }
    }
}
