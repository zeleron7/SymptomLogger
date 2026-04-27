using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using SymptomLogger.src.DTO.User;
using SymptomLogger.src.Entity;

namespace SymptomLogger.src.Controllers
{

    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly ILogger<AuthController> _logger;
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly IConfiguration _configuration;
        public AuthController(
            UserManager<User> userManager,
            SignInManager<User> signInManager,
            IConfiguration configuration,
            ILogger<AuthController> logger)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _configuration = configuration;
            _logger = logger;
        }

        // POST: api/Auth/register
        #region Register
        [HttpPost("register")]
        public async Task<IActionResult> Register(CreateUserDTO register)
        {
            // map dto to entity
            var user = new User
            {
                FirstName = register.FirstName,
                LastName = register.LastName,
                Age = register.Age,
                UserName = register.UserName,
                Email = register.Email,
            };

            var result = await _userManager.CreateAsync(user, register.Password);
            if (result.Succeeded)
            {
                // add roles if needed

                _logger.LogInformation("New user registered with email {Email}.", register.Email);
                return Ok(new { message = "Registration successful" });
            }
            _logger.LogWarning("Registration failed for email {Email}.", register.Email);
            return BadRequest(result.Errors);
        }
        #endregion

        // POST: api/Auth/login
        #region Login
        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginUserDTO login)
        {
            var user = await _userManager.FindByEmailAsync(login.Email);
            if (user == null)
            {
                _logger.LogWarning("Login attempt failed for email {Email}. User not found.", login.Email);
                return BadRequest("Invalid credentials");
            }
            var result = await _signInManager.CheckPasswordSignInAsync(
                user, login.Password, lockoutOnFailure: false);
            if (result.Succeeded)
            {
                _logger.LogInformation("User with email {Email} logged in successfully.", login.Email);
                var token = GenerateJwtToken(user);
                return Ok(new { token });
            }
            return BadRequest("Invalid credentials");
        }
        #endregion

        // POST: api/Auth/logout
        #region logout
        [HttpPost("logout")]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            _logger.LogInformation("Patient logged out.");
            return Ok(new { message = "Logout successful" });
        }
        #endregion 

        // Generate JWT token
        #region GenerateJwtToken
        private string GenerateJwtToken(User patient)
        {
            var claims = new[]
            {
            new Claim(JwtRegisteredClaimNames.Sub, patient.Id),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            new Claim(ClaimTypes.Email, patient.Email)
        };
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var token = new JwtSecurityToken(
                issuer: _configuration["Jwt:Issuer"],
                audience: _configuration["Jwt:Audience"],
                claims: claims,
                expires: DateTime.Now.AddHours(3),
                signingCredentials: creds);
            return new JwtSecurityTokenHandler().WriteToken(token);
        }
        #endregion 
    }
}
