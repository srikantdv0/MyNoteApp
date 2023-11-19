using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Notes.Services;

namespace Notes.Controllers
{
    [ApiController]
    [Route("api/[Controller]")]
    public class AuthenticationController : Controller
    {
        private readonly IConfiguration _configuration;
        private readonly INoteRepository _noteRepository;

        public AuthenticationController(IConfiguration configuration, INoteRepository noteRepository)
        {
            _configuration = configuration;
            _noteRepository = noteRepository;
        }

        [HttpPost("authenticate")]
        public async Task<ActionResult<string>> Authenticate(AuthenticationRequestBody authenticationRequestBody)
        {
            var user = await ValidateUserCredentials(authenticationRequestBody.Email
                ,authenticationRequestBody.Password);
            if (user == null)
            {
                return Unauthorized("Invalid credentials");
            }

            var securityKey = new SymmetricSecurityKey(
                Encoding.ASCII.GetBytes(_configuration["Authentication:SecretForKey"]));

            var signingCredentials = new SigningCredentials(
                securityKey, SecurityAlgorithms.HmacSha256);

            var claimsForToken = new List<Claim>();
            claimsForToken.Add(new Claim("UserId", user.Id.ToString()));
            claimsForToken.Add(new Claim("UserEmailAddress", user.Email.ToString()));
            claimsForToken.Add(new Claim("UserName", user.Name.ToString()));

            var jwtSecurityToken = new JwtSecurityToken(
                _configuration["Authentication:Issuer"],
                _configuration["Authentication:Audience"],
                claimsForToken,
                DateTime.UtcNow,
                //DateTime.UtcNow.AddHours(1),
                DateTime.UtcNow.AddSeconds(60),
                signingCredentials);

            var tokenToReturn = new JwtSecurityTokenHandler()
                .WriteToken(jwtSecurityToken);
            return Ok(tokenToReturn);

        }

        [Authorize]
        [HttpPost("logout")]
        public async Task<IActionResult> LogoutAsync()
        {
            await HttpContext.SignOutAsync();
            return Ok("success");
        }

        private async Task<NoteUserInfo?> ValidateUserCredentials(string email, string password)
        {
            var user = await _noteRepository.IsValidAsync(email,password);
            if (user == null)
            {
                return null;
            }
            return new NoteUserInfo()
            {
                Id = user.Id,
                Email = email,
                Name = user.Name
            };

        }
    }

    internal class NoteUserInfo
    {
        public int Id { get; set; }
        public string Email { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
    }

    public class AuthenticationRequestBody
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;
        [Required]
        public string Password { get; set; } = string.Empty;
    }
}

