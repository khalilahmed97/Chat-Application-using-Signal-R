using server.Models;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace server
{
    public class JWTService
    {
        private readonly string key = string.Empty;
        private readonly int duration;

        public JWTService(IConfiguration configuration)
        {
            key = configuration["JWT_CONFIG:SECRET_KEY"]!;
            duration = int.Parse(configuration["JWT_CONFIG:DURATION"]!);
        }

        public string GenerateToken(User user)
        {
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(this.key));
            var signingKey = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
                new Claim("id", user.Id.ToString()),
                new Claim("name", user.Name),
                new Claim("username", user.Username),
     /*           new Claim("avatar", user.Avatar),
                new Claim("accountStatus", user.Status.ToString()),*/
                /*new Claim("createdOn", user.CreatedOn.ToString("yyyyMMdd")),*/
            };

            var jwtToken = new JwtSecurityToken(
                issuer: "localhost",
                audience: "localhost",
                claims: claims,
                notBefore: DateTime.Now,
                expires: DateTime.Now.AddMinutes(this.duration),
                signingKey);

            return new JwtSecurityTokenHandler().WriteToken(jwtToken);
        }
    }
}
