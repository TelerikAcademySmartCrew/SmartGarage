using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using SmartGarage.WebAPI.Models;

namespace SmartGarage.Services
{
    public  class JwtService
    {
        private readonly IConfiguration configuration;
        private readonly UserManager<AppUser> userManager;
        private readonly string jwtSecret;

        public JwtService(IConfiguration configuration,
            UserManager<AppUser> userManager,
            string jwtSecret)
        {
            this.configuration = configuration;
            this.userManager = userManager;
            this.jwtSecret = jwtSecret;
        }
        
        public string GenerateJsonWebToken(AppUser user)
        {
            var authClaims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.UserName),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            var jwtSecret = this.configuration["JWT:Secret"];
            var jwtSecretBytes = Encoding.UTF8.GetBytes(jwtSecret);
            var authSigningKey = new SymmetricSecurityKey(jwtSecretBytes);

            var token = new JwtSecurityToken(
                issuer: this.configuration["JWT:ValidIssuer"],
                audience: this.configuration["JWT:ValidAudience"],
                expires: DateTime.Now.AddHours(3),
                claims: authClaims,
                signingCredentials: new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256)
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
        
        public async Task<AppUser> TryGetUserFromToken(string token)
        {
            token = token.Replace("Bearer ", string.Empty);
            var username = GetUsernameFromToken(token);
            return await this.userManager.FindByNameAsync(username);
        }

        private string? GetUsernameFromToken(string token)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(jwtSecret);

            try
            {
                SecurityToken securityToken;
                var principal = tokenHandler.ValidateToken(token, new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    ClockSkew = TimeSpan.Zero
                }, out securityToken);

                if (securityToken is JwtSecurityToken jwtSecurityToken &&
                    principal.Identity is ClaimsIdentity claimsIdentity)
                {
                    var usernameClaim = claimsIdentity.FindFirst(ClaimTypes.Name);
                    return usernameClaim?.Value;
                }
            }
            catch (SecurityTokenException)
            {
                throw new Exception("Invalid token");
            }

            return null;
        }
    }
}
