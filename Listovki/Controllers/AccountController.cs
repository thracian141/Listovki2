﻿using Listovki.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.IdentityModel.Tokens.Jwt;

namespace Listovki.Controllers {
    [ApiController]
    [Route("account")]
    public class AccountController : Controller {
        private readonly UserManager<AppUser> _userManager;
        private readonly ILogger<AccountController> _logger;

        public AccountController(UserManager<AppUser> userManager, ILogger<AccountController> logger)
        {
            _userManager = userManager;
            _logger = logger;
        }

        [HttpGet("info")]
        public async Task<IActionResult> Info() {
            var user = await _userManager.GetUserAsync(User);

            var userinfo = new {
                user.FullName,
                user.UserName,
                user.EGN,
                user.BirthDate,
                user.Email,
                user.PassedExams,
                user.FailedExams,
                user.IsAdmin
            };

            return new JsonResult(new { userinfo });
        }
        [HttpGet("username")]
        public async Task<IActionResult> Username() {
            var user = await _userManager.GetUserAsync(User);

            if (user == null) {
                _logger.LogWarning("User is null");
                return NotFound("User not found.");
            }

            return new JsonResult(new { user.UserName });
        }
        [HttpGet("email")]
        public async Task<IActionResult> Email()
        {
            var user = await _userManager.GetUserAsync(User);

            if (user == null) {
                var email = "0";
                return new JsonResult(new { email });
            } else {
                return new JsonResult(new { user.Email });
            }
        }
        [HttpGet("isadmin")]
        public async Task<IActionResult> IsAdmin() {
            var user = await _userManager.GetUserAsync(User);

            if (user == null) {
                return NotFound("User not found");
            }

            return new JsonResult(new { user.IsAdmin });
        }
    }
}
