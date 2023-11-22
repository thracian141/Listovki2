using Listovki.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Listovki.Controllers.Services {
    public class TokenService : ITokenService {
        private readonly IConfiguration _configuration;
        private readonly UserManager<AppUser> _userManager;

        public TokenService(IConfiguration configuration, UserManager<AppUser> userManager) {
            _configuration = configuration;
            _userManager = userManager;
        }

        public async Task<string> GenerateToken(AppUser user) {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.Email),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim("UserName", user.UserName),
                new Claim("FullName", user.FullName),
                new Claim("BirthDate", user.BirthDate.ToString()),
                new Claim("EGN", user.EGN),
                new Claim("IsAdmin", user.IsAdmin.ToString()),
                new Claim("PassedExams", user.PassedExams.ToString()),
                new Claim("FailedExams", user.FailedExams.ToString())
            };

            var userRoles = await _userManager.GetRolesAsync(user);

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JwtKey"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var expires = DateTime.Now.AddDays(Convert.ToDouble(_configuration["JwtExpireDays"]));

            claims.AddRange(userRoles.Select(role => new Claim(ClaimTypes.Role, role)));

            var token = new JwtSecurityToken(
                _configuration["JwtIssuer"],
                _configuration["JwtIssuer"],
                claims,
                expires: expires,
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
