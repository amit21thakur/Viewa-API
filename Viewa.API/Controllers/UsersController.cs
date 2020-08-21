using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Viewa.Models;
using Viewa.Services;

namespace Viewa.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UsersController : ControllerBase
    {
        private readonly ILogger<UsersController> _logger;
        private readonly IUserService _userService;

        public UsersController(ILogger<UsersController> logger, IUserService userService)
        {
            _logger = logger;
            _userService = userService;
        }

        [HttpPost]
        [Route("authenticate")]
        public async Task<IActionResult> Login(LoginRequest request)
        {
            try
            {
                var result = await _userService.Login(request.Username, request.Password);
                if (result.UserExists)
                {
                    if (result.IsLoginSuccessfull)
                        return Ok(result.Data);
                    else
                        return StatusCode((int)HttpStatusCode.Unauthorized);
                }
                return StatusCode((int)HttpStatusCode.BadRequest);
            }
            catch(Exception ex)
            {
                _logger.LogError(ex, "Failed while logging in.");
                return StatusCode((int)HttpStatusCode.InternalServerError);
            }
        }

        [HttpGet]
        [Authorize]
        [Route("data/{email}")]
        public async Task<IActionResult> GetUserData(string email)
        {
            var data = await _userService.GetUserData(email);
            return Ok(data);
        }
    }
}
