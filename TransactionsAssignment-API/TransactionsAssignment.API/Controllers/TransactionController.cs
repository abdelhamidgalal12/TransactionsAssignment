using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using TransactionsAssignment.Application.DTOs.TransactionDtos;
using TransactionsAssignment.Application.Interfaces;
using TransactionsAssignment.Application.IServices;
using TransactionsAssignment.Infrastructure.IServices;

namespace TransactionsAssignment.API.Controllers
{
    [ApiController]
    [Route("api/transaction")]
    public class TransactionController : Controller
    {
        private readonly ITransactionService _transactionService;
        private readonly IEncryptionService _encryptionService;
        private readonly IJwtService _jwtService;

        public TransactionController(IJwtService jwtService, ITransactionService transactionService, IEncryptionService encryptionService)
        {
            _transactionService=transactionService;
            _encryptionService=encryptionService;
            _jwtService = jwtService;

        }
        [HttpGet("generatekey")]
        public async Task<TransactionEncryptKey> GenerateKey()
        {
            return await  _transactionService.GenerateEncryptionKey();
        }
        [HttpPost("process")]
        [Authorize]
        public IActionResult ProcessTransaction([FromBody] dynamic request)
        {
            try
            {
                if (!Request.Headers.ContainsKey("Authorization"))
                {
                    return Unauthorized(new { Message = "Authorization header is missing." });
                }
                var authorizationHeader = Request.Headers["Authorization"].ToString();

                if (!authorizationHeader.StartsWith("Bearer ", StringComparison.OrdinalIgnoreCase))
                {
                    return Unauthorized(new { Message = "Invalid token format." });
                }
                var token = authorizationHeader.Substring("Bearer ".Length).Trim();
                if ( _jwtService.ValidateToken(token))
                {
                    var decryptedDataJson = _encryptionService.Decrypt(request);
                    var transaction = JsonConvert.DeserializeObject<TransactionRequest>(decryptedDataJson);
                    var useCase = _transactionService.ProcessTransactionAsync(transaction);
                    return Ok(useCase);
                }
                {
                    return Unauthorized(new { Message = "Invalid or expired token." });
                }
            }
            catch (SecurityTokenExpiredException)
            {
                return Unauthorized(new { Message = "Token has expired. Please log in again." });
            }
            catch (Exception ex)
            {
                return BadRequest(new { Message = ex.Message });
            }
        }
      

    }
}
