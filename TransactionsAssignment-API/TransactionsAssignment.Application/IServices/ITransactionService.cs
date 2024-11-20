using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TransactionsAssignment.Application.DTOs.TransactionDtos;
using TransactionsAssignment.Domain.Entities;

namespace TransactionsAssignment.Application.Interfaces
{
    public interface ITransactionService
    {
        Task<TransactionResponse> ProcessTransactionAsync(TransactionRequest transaction);
        Task<TransactionEncryptKey> GenerateEncryptionKey();
    }
}
