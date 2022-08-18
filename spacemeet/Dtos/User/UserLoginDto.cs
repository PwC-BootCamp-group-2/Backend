using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace spacemeet.Dtos.User
{
    public class UserLoginDto
    {
        public string email { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
    }
}