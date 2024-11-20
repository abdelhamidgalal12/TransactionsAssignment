using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using TransactionsAssignment.Application.DTOs.TransactionDtos;
using TransactionsAssignment.Infrastructure.Helpers;
using TransactionsAssignment.Infrastructure.IServices;

namespace TransactionsAssignment.Infrastructure.Services
{
    public class EncryptionService : IEncryptionService
    {

        public string Decrypt(dynamic request)

        {
            var encryptedData = parse(request, "EncryptedTransactionData");
            var encryptionKey = parse(request, "EncryptionKey");

            return CryptoJsStaticHelper.ProcessDecryption(encryptedData, encryptionKey);
        }
        public string parse(dynamic obj, string prop)
        {
            JsonElement value = obj.GetProperty(prop);
            return value.ToString();
        }

       
    }
}
