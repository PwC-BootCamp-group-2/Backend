using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using spacemeet.Dtos.User;

namespace spacemeet.Interfaces
{
    public interface UserResponse
    {
        public string token { get; set; }

        public string email { get; set; }
        public string companyName { get; set; }
        public string phoneNumber { get; set; }
        public string role { get; set; }

  }
}