using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using GDGproj.Models;
using GDGproj.Data;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Org.BouncyCastle.Crypto.Generators;

namespace GDGproj.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : Controller
    {
        private readonly AppDbContext _context;
        private readonly IConfiguration _configuration;

        public AuthController(AppDbContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }

        // Register
        [HttpPost("register")]
        public async Task<ActionResult<User>> Register([FromBody] User user)
        {
            if (_context.Users.Any(u => u.Email == user.Email))
                return BadRequest("Email already exists");

            user.PasswordHash = BCrypt.Net.BCrypt.HashPassword((string)user.Password); // Hash the password
            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            return Ok(new { Message = "User registered successfully." });
        }

        // Login
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] User loginUser)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == loginUser.Email);
            if (user == null || !BCrypt.Net.BCrypt.Verify((string)loginUser.Password, user.PasswordHash))
                return Unauthorized("Invalid Email or Password");

            var token = GenerateJwtToken(user);
            return Ok(new { Token = token });
        }

        // Generates JWT Token
        private string GenerateJwtToken(User user)
        {
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.Email),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim("UserId", user.Id.ToString()),
                new Claim("Role", user.Role)
            };

            var token = new JwtSecurityToken(
                issuer: _configuration["Jwt:Issuer"],
                audience: _configuration["Jwt:Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddHours(2),
                signingCredentials: credentials
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
