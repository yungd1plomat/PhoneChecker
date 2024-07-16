using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PhoneChecker.Abstractions;
using PhoneChecker.Models;

namespace PhoneChecker.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PhoneController : ControllerBase
    {
        private readonly IPhoneNumberChecker _checker;

        public PhoneController(IPhoneNumberChecker checker) 
        {
            _checker = checker;
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
    }
}
