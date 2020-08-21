using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Viewa.Db;
using Viewa.Models;

namespace Viewa.Services
{
    public class UserService : IUserService
    {
        private readonly ViewaContext _context;
        private readonly IConfiguration _configuration;
        private readonly IMapper _autoMapper;
        public UserService(ViewaContext context, IConfiguration configuration, IMapper mapper)
        {
            _context = context;
            _configuration = configuration;
            _autoMapper = mapper;
        }

        public async Task<UserData> GetUserData(string username)
        {
            var user = await _context.Users.FirstOrDefaultAsync(x => x.Username.ToLower() == username.ToLower());
            
            if(user!=null)
            {
                return _autoMapper.Map<UserData>(user);
            }
            return new UserData();
        }

        public async Task<LoginResponse> Login(string username, string password)
        {
            var loginResponse = new LoginResponse();
            var user = await _context.Users.FirstOrDefaultAsync(x => x.Username.ToLower() == username.ToLower());
            if (user != null)
            {
                loginResponse.UserExists = true;
                if (user.Password == password)
                {
                    loginResponse.IsLoginSuccessfull = true;

                    var minsToExpiry = _configuration.GetSection("TokenTimeToExpiryInMins").Get<double>();
                    var tokenSecretKey = _configuration.GetSection("TokenSecurityKey").Value;
                    var claims = new List<Claim>
                    {
                        new Claim(ClaimTypes.Name, user.Username),
                        new Claim(ClaimTypes.NameIdentifier, user.UserId.ToString()),
                        new Claim(JwtRegisteredClaimNames.Nbf, new DateTimeOffset(DateTime.Now).ToUnixTimeSeconds().ToString()),
                        new Claim(JwtRegisteredClaimNames.Exp, new DateTimeOffset(DateTime.Now.AddMinutes(minsToExpiry)).ToUnixTimeSeconds().ToString())
                    };

                    var token = new JwtSecurityToken(
                        new JwtHeader(
                            new SigningCredentials(new SymmetricSecurityKey(Encoding.UTF8.GetBytes(tokenSecretKey)),
                            SecurityAlgorithms.HmacSha256)),
                        new JwtPayload(claims));
                    loginResponse.Data.Token = new JwtSecurityTokenHandler().WriteToken(token);
                }
            }
            return loginResponse;
        }


    }
}
