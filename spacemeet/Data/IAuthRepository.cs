using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using spacemeet.Dtos.User;
using spacemeet.Interfaces;

namespace spacemeet.Data
{
    public interface IAuthRepository
    {
        Task<ServiceResponse<int>> Register(User user, string password);
        Task<ServiceResponse<User>> Login(string email, string password);
        Task<bool> UserExists(string email);

    }
}