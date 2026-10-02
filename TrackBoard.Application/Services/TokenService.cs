using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using TrackBoard.Application.Interfaces;
using TrackBoard.Domain.Entities;

namespace TrackBoard.Application.Services
{
    public class TokenService : ITokenService
    {
        private readonly IConfiguration _configuration;

        public TokenService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public string GenerateToken(User user)
        {
            var jwtSettings = _configuration.GetSection("Jwt");
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings["Key"]!));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
                new Claim("UserId",user.UserId.ToString()),
                new Claim("OrgId",user.OrgId?.ToString() ?? ""),
                new Claim("Role",user.Role ?? ""),
                new Claim("UserName",user.UserName),
                new Claim("FirstName",user.FirstName),
                new Claim("LastName",user.LastName),
                new Claim("Email",user.Email)
            };

            var token = new JwtSecurityToken(
                jwtSettings["Issuer"],
                jwtSettings["Audience"],
                claims,
                expires: DateTime.Now.AddMinutes(int.Parse(jwtSettings["ExpireInMinutes"]!)),
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
