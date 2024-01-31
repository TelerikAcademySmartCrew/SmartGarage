using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.Extensions.Configuration;
using SmartGarage.WebAPI.Models;

namespace SmartGarage.Services
{
    public  class JwtService
    {
        private readonly IConfiguration configuration;

        public JwtService(IConfiguration configuration)
        {
            this.configuration = configuration;
        }

        // public string GenerateToken(string username, bool isAdmin)
        // {
        //     var tokenHandler = new JwtSecurityTokenHandler();
        //     var key = Encoding.ASCII.GetBytes(secretKey);
        //
        //     var claims = new[]
        //     {
        //         new Claim(ClaimTypes.Name, username),
        //         new Claim("Admin", isAdmin.ToString())
        //     };
        //
        //     var tokenDescriptor = new SecurityTokenDescriptor
        //     {
        //         Subject = new ClaimsIdentity(claims),
        //         Expires = DateTime.UtcNow.AddHours(1),
        //         SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
        //     };
        //
        //     var token = tokenHandler.CreateToken(tokenDescriptor);
        //     return tokenHandler.WriteToken(token);
        // }
        
        public string GenerateJsonWebToken(AppUser user)
        {
            var authClaims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.UserName),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            string jwtSecret = this.configuration["JWT:Secret"];
            byte[] jwtSecretBytes = Encoding.UTF8.GetBytes(jwtSecret);
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
    }
}
