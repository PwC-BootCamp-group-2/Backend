using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using spacemeet.Data;
using spacemeet.Dtos.User;

namespace spacemeet.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthRepository _authrepo;
        public AuthController(IAuthRepository authrepo)
        {
            _authrepo = authrepo;
            
        }

        [HttpPost("register")]
        public async Task<ActionResult<ServiceResponse<int>>> Register(UserRegisterDto request)
        {
            var response = await _authrepo.Register(
                new User {email = request.email, phoneNumber = request.phoneNumber, companyName = request.companyName, role = request.role, address = request.address, fullName= request.fullName}, request.Password
            );
            if(!response.Success) {
                return BadRequest(response);
            }
            return Ok(response);
        }
        [HttpPost("login")]
        public async Task<ActionResult<ServiceResponse<string>>> Login(UserLoginDto request)
        {
            var response = await _authrepo.Login(request.email, request.Password);
            if(!response.Success) {
                return BadRequest(response);
            }
            return Ok(response);
        }
        [HttpPost("verify")]
        public async Task<ActionResult<ServiceResponse<string>>> Verify(string token)
        {
            var response = await _authrepo.Verify(token);
            if(!response.Success) {
                return BadRequest(response);
            }
            return Ok(response);
        }
    }
}