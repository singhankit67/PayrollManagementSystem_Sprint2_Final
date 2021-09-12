using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using PayrollManagementSystem_API.Models;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace PayrollManagementSystem_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoginController : Controller
    {
        private IConfiguration _config;

        public LoginController(IConfiguration config)
        {
            _config = config;
        }
        [HttpGet]
        public IActionResult Login(string username, string pass)
        {
            LoginDetails login = new LoginDetails();
            login.UserName = username;
            login.Password = pass;
            IActionResult response = Unauthorized();

            var user = AuthenticateUser(login);
            if(user != null)
            {
                var tokenStr = GenerateJSONWebToken(user);
                response = Ok(new { token = tokenStr });
            }
            return response;
        }

        private LoginDetails AuthenticateUser(LoginDetails login)
        {
            LoginDetails user = null;
            if(login.UserName == "ankit" && login.Password == "123")
            {
                user = new LoginDetails { UserName = "Ankit", Password = "123" };
            }
            return user;
        }


        private string GenerateJSONWebToken(LoginDetails loginInfo)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));
            var credentails = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub,loginInfo.UserName),
                new Claim(JwtRegisteredClaimNames.Jti,Guid.NewGuid().ToString())
            };

            var token = new JwtSecurityToken(
                issuer: _config["Jwt:Issuer"],
                audience: _config["Jwt:Issuer"],
                claims,
                expires: DateTime.Now.AddMinutes(120),
                signingCredentials: credentails);

            var encodeToken = new JwtSecurityTokenHandler().WriteToken(token);
            return encodeToken;

        }
        [Authorize]
        [HttpPost("Post")]
        public string Post()
        {
            var Identity = HttpContext.User.Identity as ClaimsIdentity;
            IList<Claim> claim = Identity.Claims.ToList();
            var userName = claim[0].Value;
            return "Welcome To : " + userName;
        }
        [Authorize]
        [HttpGet("GetValue")]
        public ActionResult<IEnumerable<string>> Get()
        {
            return new string[] { "Value1 ", "Value2", "Value3" };
        }
        public IActionResult Index()
        {
            return View();
        }
    }
}
