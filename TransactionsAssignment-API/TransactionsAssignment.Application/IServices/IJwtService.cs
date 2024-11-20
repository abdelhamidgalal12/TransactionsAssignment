using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using TransactionsAssignment.Application.DTOs.TransactionDtos;

namespace TransactionsAssignment.Application.IServices
{
    public interface IJwtService
    {
        string GenerateToken(string userId, string userName, IEnumerable<string> roles);
        bool ValidateToken(string token);
        string GetUserIdFromToken(string token);
    }
}
