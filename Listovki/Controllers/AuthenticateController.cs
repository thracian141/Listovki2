using Listovki.Models;
using Listovki.Models.DataModels;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Listovki.Controllers.Services;

namespace Listovki.Controllers {
    [ApiController]
    [Route("authenticate")]
    public class AuthenticateController : Controller {
        private readonly SignInManager<AppUser> _signInManager;
        private readonly UserManager<AppUser> _userManager;
        private readonly ILogger<AuthenticateController> _logger;
        private readonly ITokenService _tokenService;

        public AuthenticateController(SignInManager<AppUser> signInManager,UserManager<AppUser> userManager,
            ILogger<AuthenticateController> logger, ITokenService tokenService) {
            _signInManager = signInManager;
            _userManager = userManager;
            _logger = logger;
            _tokenService = tokenService;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterInputModel model) {
            if (!ModelState.IsValid) {
                return BadRequest(ModelState);
            }

            var user = new AppUser {
                Email = model.Email,
                UserName = model.UserName,
                FullName = model.FullName,
                BirthDate = DateTime.Parse(model.BirthDate),
                EGN = model.EGN,
                PassedExams = 0,
                FailedExams = 0,
                IsAdmin = false,
                ProfilePictureURL = null
            };

            var result = await _userManager.CreateAsync(user, model.Password);

            if (!result.Succeeded) {
                return BadRequest(result.Errors);
            }

            var token = await _tokenService.GenerateToken(user);

            // create a cookie with the token
            var cookieOptions = new CookieOptions {
                HttpOnly = true,
                Expires = DateTime.UtcNow.AddDays(1)
            };
            Response.Cookies.Append("token", token, cookieOptions);

            return new JsonResult(new { token });
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginInputModel model) {
            if (!ModelState.IsValid) {
                return BadRequest(ModelState);
            }

            var user = await _userManager.FindByEmailAsync(model.Email);
            if (user == null || !await _userManager.CheckPasswordAsync(user, model.Password)) {
                return Unauthorized();
            }

            var token = await _tokenService.GenerateToken(user);

            // create a cookie with the token
            var cookieOptions = new CookieOptions {
                HttpOnly = true,
                Expires = DateTime.UtcNow.AddDays(1)
            };
            Response.Cookies.Append("token", token, cookieOptions);

            return new JsonResult(new { token });
        }

        //generate a logout method from my token based authentication system
        [HttpPost("logout")]
        public async Task<IActionResult> Logout() {
            await _signInManager.SignOutAsync();
            Response.Cookies.Delete("token");
            return Ok();
        }
    }
}
