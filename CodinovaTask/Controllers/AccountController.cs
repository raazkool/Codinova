using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using CodinovaTask.Helpers;
using CodinovaTask.Model;
using CodinovaTask.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace CodinovaTask.Controllers
{
    [Route("api/[controller]")]
    [Authorize]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private IUserService _userService;
        private readonly AppSettings _appSettings;
        public AccountController(IUserService userService, IOptions<AppSettings> appSettings)
        {
            _userService = userService;
            _appSettings = appSettings.Value;
        }

        [AllowAnonymous]
        [HttpPost]
        [Route("Login")]
        public IActionResult Login([FromBody]UserRegistration userRegistration)
        {
            var result = _userService.Authenticate(userRegistration.Username, userRegistration.Password);
            if (result == null)
                return BadRequest(new { message = "Username or password is invalid." });

            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_appSettings.Secret);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Name, result.UserId.ToString())
                }),
                Expires = DateTime.UtcNow.AddDays(10),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            var tokenString = tokenHandler.WriteToken(token);
            return Ok(new
            {
                Id = result.UserId,
                Username = result.UserName,
                FirstName = result.FirstName,
                LastName = result.LastName,
                Token = tokenString
            });
        }

       
       
        [Route("Registration")]
        [AllowAnonymous]
        [HttpPost]
        public IActionResult UserRegistration([FromBody]UserRegistration userRegistration)
        {
            try
            {
                UserDetails _userInfo = new UserDetails
                {
                    FirstName = userRegistration.FirstName,
                    LastName = userRegistration.LastName,
                    UserName = userRegistration.Username,
                };
                _userService.Create(_userInfo, userRegistration.Password);
                return Ok("Registration Successfully.");
            }
            catch (AppException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpGet]
        public IActionResult GetAll()
        {
            var users = _userService.GetAllUsers();
            return Ok(users);
        }
    }
}
