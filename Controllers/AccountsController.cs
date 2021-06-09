using EFWebApplicationWithAuthorization.Models;
using EFWebApplicationWithAuthorization.Services;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace EFWebApplicationWithAuthorization.Controllers
{
    [Route("api/login")]
    [ApiController]
    public class AccountsController : ControllerBase
    {
        private IConfiguration _con;
        private IDbService _dbService;
        public AccountsController(IConfiguration con, IDbService dbService)
        {
            _con = con;
            _dbService = dbService;
        }
        [HttpPost("register")]
        public IActionResult Register(LoginRequest loginRequest)
        {
            _dbService.MakeRegistration(loginRequest);

            return Ok("Zarejestrowano klienta");
        }
        [HttpPost]
        public IActionResult Login(LoginRequest loginRequest)
        {
            var IsPasswordCorrect=_dbService.CheckPasswordCorrectness(loginRequest);

            if (!IsPasswordCorrect)
                return Unauthorized("Login or password is incorrect");

            var accessToken = _dbService.GenerateAccessToken();
            var refreshToken=_dbService.GenerateRefreshToken();
            if (refreshToken == null)
                return BadRequest("Something went wrong!");

            return Ok(new
            {
                accessToken = new JwtSecurityTokenHandler().WriteToken(accessToken),
                refreshToken = refreshToken
            });
        }
        [HttpPost("accessToken")]
        public IActionResult GetAccessTokenByRefreshToken(string refreshToken)
        {

            var IsCorrect=_dbService.IsRefreshTokenCorrect(refreshToken);
            if (!IsCorrect)
                return Unauthorized("Refresh token is incorrect");

            var accessToken = _dbService.GenerateAccessToken();

            return Ok(new
            {
                accessToken= new JwtSecurityTokenHandler().WriteToken(accessToken)
            });
        }
    }
}
