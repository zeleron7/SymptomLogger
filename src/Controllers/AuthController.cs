using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using SymptomLogger.src.DTO;
using SymptomLogger.src.Entity;

namespace SymptomLogger.src.Controllers
{

    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly UserManager<Patient> _userManager;
        private readonly SignInManager<Patient> _signInManager;
        private readonly IConfiguration _configuration;
        public AuthController(
            UserManager<Patient> userManager,
            SignInManager<Patient> signInManager,
            IConfiguration configuration)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _configuration = configuration;
        }

        // POST: api/Auth/register
        #region Register
        [HttpPost("register")]
        public async Task<IActionResult> Register(RegisterPatientDTO model)
        {
            var patient = new Patient
            {
                UserName = model.UserName,
                Email = model.Email,
                FirstName = model.FirstName,
                LastName = model.LastName
            };
            var result = await _userManager.CreateAsync(patient, model.Password);
            if (result.Succeeded)
            {
                // add roles if needed

                return Ok(new { message = "Registration successful" });
            }
            return BadRequest(result.Errors);
        }
        #endregion

        // POST: api/Auth/login
        #region Login
        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginPatientDTO model)
        {
            var user = await _userManager.FindByEmailAsync(model.Email);
            if (user == null)
            {
                return BadRequest("Invalid credentials");
            }
            var result = await _signInManager.CheckPasswordSignInAsync(
                user, model.Password, lockoutOnFailure: false);
            if (result.Succeeded)
            {
                var token = GenerateJwtToken(user);
                return Ok(new { token });
            }
            return BadRequest("Invalid credentials");
        }
        #endregion

        // Generate JWT token
        #region GenerateJwtToken
        private string GenerateJwtToken(Patient user)
        {
            var claims = new[]
            {
            new Claim(JwtRegisteredClaimNames.Sub, user.Email),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            new Claim(ClaimTypes.NameIdentifier, user.Id)
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
