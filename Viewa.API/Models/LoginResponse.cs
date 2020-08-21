using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Threading.Tasks;

namespace Viewa.Models
{
    public class LoginResponse
    {
        public LoginResponse()
        {
            UserExists = false;
            IsLoginSuccessfull = false;
            Data = new Data();
        }
        public Data Data { get; set; }
        public bool UserExists { get; set; }
        public bool IsLoginSuccessfull { get; set; }
    }

    public class Data
    {
        public string Token { get; set; }
    }
}
