using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace spacemeet.Dtos.User
{
    public class UserRegisterDto
    {
        public string Username { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public string phoneNumber { get; set; } = string.Empty;
        public string companyName { get; set; } = string.Empty;
    }
}