using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using PhoneChecker.Abstractions;
using PhoneChecker.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace PhoneChecker.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PhoneController : ControllerBase
    {
        private readonly IPhoneNumberChecker _checker;

        private readonly JwtOptions _jwtOptions;

        public PhoneController(IPhoneNumberChecker checker,
            JwtOptions jwtOptions) 
        {
            _checker = checker;
            _jwtOptions = jwtOptions;
        }

        [HttpPost("check")]
        public IActionResult Check(Phone phone)
        {
            var isBlocked = _checker.IsBlocked(phone.Number);
            if (isBlocked)
            {
                return BadRequest("block");
            }
            return Ok("ok");
        }

        [HttpPost("add")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public IActionResult Add(Phone phone)
        {
            _checker.Insert(phone.Number);
            return Ok(phone.Number);
        }

        [HttpPost("auth")]
        public IActionResult Auth()
        {
            // Здесь будет логика авторизации (пока просто выдаем токены с рандомным Id)
            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            };
            var jwt = new JwtSecurityToken(
                issuer: _jwtOptions.Issuer,
                expires: DateTime.MaxValue,
                audience: _jwtOptions.Audience,
                claims: claims,
                signingCredentials: new SigningCredentials(_jwtOptions.SymmetricSecurityKey, SecurityAlgorithms.HmacSha256));
            var token = new JwtSecurityTokenHandler().WriteToken(jwt);
            return Ok(token);
        }
    }
}
