using System.Security.Cryptography;
using TransactionsAssignment.Application.DTOs.TransactionDtos;
using TransactionsAssignment.Application.Interfaces;

namespace TransactionsAssignment.Application.Services
{
    public class TransactionService : ITransactionService
    {
        public async Task<TransactionResponse> ProcessTransactionAsync(TransactionRequest transaction)
        {
            if (!transaction.Validate())
                throw new ArgumentException("Invalid transaction data");

            var approvalCode = new Random().Next(100000, 999999).ToString();
            var dateTime = DateTime.UtcNow.ToString("yyyyMMddHHmmss");
            return new TransactionResponse
            {
                ResponseCode = "00",
                Message = "Success",
                ApprovalCode = approvalCode, 
                DateTime = dateTime
            };
        }
        public async Task<TransactionEncryptKey> GenerateEncryptionKey()
        {
            using var rng = new RNGCryptoServiceProvider();
            var key = new byte[32];
            rng.GetBytes(key);
            var keyBase64 = Convert.ToBase64String(key);
            return new TransactionEncryptKey {Key= keyBase64 };
        }
    }
}
