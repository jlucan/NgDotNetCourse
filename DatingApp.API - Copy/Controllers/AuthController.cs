using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using DatingApp.API.Data;
using DatingApp.API.DTOs;
using DatingApp.API.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace DatingApp.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthRepository _repos;
        private readonly IConfiguration _config;

        public AuthController(IAuthRepository repos, IConfiguration config)
        {
            _config = config;
            _repos = repos;
        }

        [HttpGet]
        public async Task<IActionResult> GetUsers()
        {
            List<User> users = await _repos.GetUsers();
            return Ok(users);
        }


        [HttpPost("register")]
        public async Task<IActionResult> Register(UserRegisterDTO userRegisterDTO)
        {
            string userName = userRegisterDTO.UserName.ToLower();

            if (await _repos.UserExists(userName))
            {
                return BadRequest("User name already exists.");
            }

            User newUser = new User { UserName = userName };
            User createdUser = await _repos.Register(newUser, userRegisterDTO.Password);

            return StatusCode(201);
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(UserLoginDTO userLoginDTO)
        {
            var user = await _repos.Login(userLoginDTO.UserName.ToLower(), userLoginDTO.Password);

            if (user == null) return Unauthorized();

            var claims = new []
            {
              new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
              new Claim(ClaimTypes.Name, user.UserName) 
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config.GetSection("AppSettings:Token").Value));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.Now.AddDays(1),
                SigningCredentials = creds
            };
            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDescriptor);

            return Ok(new { token = tokenHandler.WriteToken(token) });
        }
    }
}