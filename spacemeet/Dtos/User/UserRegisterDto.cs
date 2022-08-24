using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace spacemeet.Dtos.User
{
    public class UserRegisterDto
    {
        public string email { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public string phoneNumber { get; set; } = string.Empty;
        public string companyName { get; set; } = string.Empty;
        public string role { get; set; } = string.Empty;
        public string address { get; set; } = string.Empty;

  }
}