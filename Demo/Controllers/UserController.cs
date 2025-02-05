using Demo.Data;
using Demo.DTO;
using Demo.Model;
using Demo.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Cryptography;
using System.Text;

namespace Demo.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly JwtTokenGenerator _jwtTokenGenerator;
        public UserController(ApplicationDbContext dbContext, JwtTokenGenerator jwtTokenGenerator)
        {
            _dbContext = dbContext;
            _jwtTokenGenerator = jwtTokenGenerator;
        }
        [HttpPost]
        [Route("signup")]
        public IActionResult PostUser([FromBody] UserDTO sIgnUpDTO)
        {
            var users = new User
            {
              UserName= sIgnUpDTO.username,
              Password= sIgnUpDTO.password,

            };
            _dbContext.Users.Add(users);
            _dbContext.SaveChanges();
            return Ok(users);
        }
        [HttpPost]
        [Route("login")]
        public IActionResult LogIN([FromBody] UserDTO logInDto)
        {
            int userId = _dbContext.Users
                 .Where(u => u.UserName == logInDto.username && u.Password == HashPassword(logInDto.password))
                 .Select(u => u.UserID)
                 .FirstOrDefault();

            var token = _jwtTokenGenerator.GenerateJwtToken(logInDto);
            return Ok(token);
        }
        [Authorize]
        [HttpGet("getuser")]
        public IActionResult getuser()
        {
            return Ok("this is my data");
        }
        // Hash the Password
        private string HashPassword(string password)
        {
            using var sha256 = SHA256.Create();
            var hashedBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
            return Convert.ToBase64String(hashedBytes);
        }
    }
}
