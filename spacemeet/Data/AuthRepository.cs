using System;
using System.Collections.Generic;
using System.Drawing;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using MimeKit;
using MimeKit.Text;


namespace spacemeet.Data
{
    public class AuthRepository : IAuthRepository
    {
        private readonly spacemeetContext _context;
        private readonly IConfiguration _configuration;
        public AuthRepository(spacemeetContext context, IConfiguration configuration)
        {
            _configuration = configuration;
            _context = context;
            
        }
        public async Task<ServiceResponse<User>> Login(string email, string password)
        {
            var response = new ServiceResponse<User>();
            var user = await _context.Users
                .FirstOrDefaultAsync(u => u.email.ToLower().Equals(email.ToLower()));

            if (user == null)
            {
                response.Success = false;
                response.Message = "Invalid credentials";
            }
            else if (!VerifyPasswordHash(password, user.passwordHash, user.passwordSalt))
            {
                response.Success = false;
                response.Message = "Invalid credentials.";
            }
            else
            {
        response.Token = CreateToken(user);
        response.Data = user;
        // response.Data.token = CreateToken(user);
        // response.Data.email = user.email;
        // response.Data.companyName = user.companyName;
        // response.Data.phoneNumber = user.phoneNumber;
        // response.Data.role = user.role;
      }
            return response;
        }

        public async Task<ServiceResponse<int>> Register(User user, string password)
        {
            
            ServiceResponse<int> response = new ServiceResponse<int>();
            if (await UserExists(user.email))
            {
                response.Success = false;
                response.Message = "User already exists.";
                return response;
            }

            CreatePasswordHash(password, out byte[] passwordHash, out byte[] passwordSalt);

            user.passwordHash = passwordHash;
            user.passwordSalt = passwordSalt;
            user.VerificationToken = CreateRandomToken();

            
            _context.Users.Add(user);
            await _context.SaveChangesAsync();
            
            //Initializing Users Wallet
            Wallet userWallet = new Wallet() { UserId = user.Id, CreatedAt = DateTime.Now, UpdatedAt = DateTime.Now };
            _context.Wallets.Add(userWallet);
            await _context.SaveChangesAsync();

            //Getting Response
            response.Data = user.Id;
            SendVerifyEmail(user.email, user.VerificationToken);
            return response;
        }

        public async Task<ServiceResponse<string>> Verify(string token)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.VerificationToken == token);
            ServiceResponse<string> response = new ServiceResponse<string>();
            if (user == null)
            {
                response.Success = false;
                response.Message = "Invalid Token.";
                return response;
            }
            user.VerifiedAt = DateTime.Now;
            await _context.SaveChangesAsync();

            response.Message = "User Verified";
            return response;
        }

        public async Task<bool> UserExists(string username)
        {
            if (await _context.Users.AnyAsync(u => u.email.ToLower() == username.ToLower()))
            {
                return true;
            }
            return false;
        }

        private void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
        {
            using (var hmac = new System.Security.Cryptography.HMACSHA512())
            {
                passwordSalt = hmac.Key;
                passwordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
            }
        }
        private bool VerifyPasswordHash(string password, byte[] passwordHash, byte[] passwordSalt)
        {
            using (var hmac = new System.Security.Cryptography.HMACSHA512(passwordSalt))
            {
                var computeHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
                return computeHash.SequenceEqual(passwordHash);
            }
        }
        private string CreateToken(User user)
        {
            List<Claim> claims = new List<Claim> {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Name, user.email),
                new Claim(ClaimTypes.Role, user.role)
            };

            SymmetricSecurityKey key = new SymmetricSecurityKey(System.Text.Encoding.UTF8
                .GetBytes(_configuration.GetSection("AppSettings:Token").Value));

            SigningCredentials creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

            SecurityTokenDescriptor tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.Now.AddDays(1),
                SigningCredentials = creds
            };

            JwtSecurityTokenHandler tokenHandler = new JwtSecurityTokenHandler();
            SecurityToken token = tokenHandler.CreateToken(tokenDescriptor);

            return tokenHandler.WriteToken(token);
        }
        private string CreateRandomToken()
        {
            return Convert.ToHexString(RandomNumberGenerator.GetBytes(64));
        }
        public void SendVerifyEmail(string recieverEmail, string token)
        {
      string link = "https://www.google.com/";
      ServiceResponse<string> response = new ServiceResponse<string>();
      string body = "<h2>Kindly verfiy your email</h2> <a href={}><button>Click Here To Verify</button></a>";
      var email = new MimeMessage();
      email.From.Add(MailboxAddress.Parse("ayoolaanibabs0@gmail.com"));
      email.To.Add(MailboxAddress.Parse("hakeemanibaba@yahoo.com"));
      email.Subject = "Space Meet - Verify Your Email";
            email.Body = new TextPart(TextFormat.Html) { Text = body };
            using var smtp = new SmtpClient();
            smtp.Connect("smtp.gmail.com", 465, true);
            smtp.Authenticate("ayoolaanibabs0@gmail.com", "jxwtarvkivovjomz");
            smtp.Send(email);
            smtp.Disconnect(true);
    }
  }
}