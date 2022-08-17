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
                new User {email = request.Username, phoneNumber = request.phoneNumber, companyName = request.companyName}, request.Password
            );
            if(!response.Success) {
                return BadRequest(response);
            }
            return Ok(response);
        }
    }
}