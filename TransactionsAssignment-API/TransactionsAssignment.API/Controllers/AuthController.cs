using Microsoft.AspNetCore.Mvc;
using TransactionsAssignment.Application.DTOs.AuthDtos;
using TransactionsAssignment.Application.IServices;

namespace TransactionsAssignment.API.Controllers
{
    [ApiController]
    [Route("api/auth")]
    public class AuthController : Controller
    {
        private readonly IJwtService _jwtService;
        private IConfiguration _config;


        public AuthController(IJwtService jwtService, IConfiguration config)
        {
            _jwtService = jwtService;
            _config = config;
        }
        [HttpPost]
        public IActionResult Authenticate([FromBody] Login credentials)
        {
            if (credentials.UserName == "user" && credentials.Password == "password")
            {
                var token = _jwtService.GenerateToken("1", credentials.UserName, new List<string> { "Admin" });
                return Ok(new { Token = token });
            }
            return Unauthorized();
        }
    }
}
