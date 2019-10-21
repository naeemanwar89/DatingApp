using DatingApp.API.Data.Repositry;
using DatingApp.API.Dtos;
using DatingApp.API.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace DatingApp.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController :ControllerBase
    {        
        public AuthRepositry _repo { get; }
        public IConfiguration _config { get; }

        public AuthController(AuthRepositry rep, IConfiguration configuration)
        {
            _repo = rep;
            _config = configuration;
        }
        [HttpPost("register")]
        public async Task<IActionResult> Register(UserRegisterDto userRegisterDto )
        {
            userRegisterDto.username = userRegisterDto.username.ToLower();
            if (await _repo.UserExists(userRegisterDto.username))
                return BadRequest("Useralready exists");
            var UsertoCreate = new User
            {
                UserName = userRegisterDto.username
            };
            var createdUser = await _repo.Register(UsertoCreate,userRegisterDto.password);
            return StatusCode(201);
        }
        [HttpPost("login")]
        public async Task<IActionResult> Login(UserLoginDto userLoginDto)
        {
            var userFromRepo = await _repo.Login(userLoginDto.username.ToLower(), userLoginDto.password);
            if (userFromRepo == null)
                return Unauthorized();
            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier,userFromRepo.Id.ToString()),
                new Claim(ClaimTypes.Name,userFromRepo.UserName)
            };
            var key = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes
                (_config.GetSection("SystemSettings:KeyIssuer").Value));

            var credentials = new SigningCredentials(key,SecurityAlgorithms.HmacSha512Signature);
            var tokenDescripter = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.Now.AddDays(999),
                SigningCredentials = credentials,
            };
            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDescripter);

            return Ok(new
            {
                token = tokenHandler.WriteToken(token)
            }) ;
        }
    }
}
