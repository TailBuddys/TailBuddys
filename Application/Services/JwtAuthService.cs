using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using TailBuddys.Application.Interfaces;
using TailBuddys.Core.Models;

namespace TailBuddys.Application.Services
{
    public class JwtAuthService : IAuth
    {
        public string GenerateToken(User user)
        {
            List<Claim> claims = new List<Claim>
            {
                new Claim("id", user.Id),
                new Claim("IsAdmin", user.IsAdmin.ToString()),
            };
            foreach (Dog dog in user.Dogs)
            {
                claims.Add(new Claim("DogId", dog.Id));
            }
            SymmetricSecurityKey securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("31cb3b1a-f4f3-466e-9099-d4f49a0dd4b8"));
            SigningCredentials credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            JwtSecurityToken token = new JwtSecurityToken(
                issuer: "TailBuddysServer",
                audience: "TailBuddysApp",
                claims: claims,
                signingCredentials: credentials
                );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
