using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using TestWebAPI.Config;

namespace TestWebAPI.Middlewares
{
    public class JWTHelper : IJWTHelper
    {
        private readonly TokenSettings _tokenSettings;

        public JWTHelper(IOptions<TokenSettings> tokenSettings)
        {
            _tokenSettings = tokenSettings.Value;
        }

        public async Task<string> GenerateJWTToken(int id, string roleCode, DateTime expire)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var secretKeyBytes = Encoding.UTF8.GetBytes(_tokenSettings.Secret);
            var claims = new[]
            {
                new Claim("id", id.ToString()),
                new Claim("roleCode", roleCode.ToString()),
                new Claim("iat", DateTimeOffset.UtcNow.ToUnixTimeSeconds().ToString()),
            };

            var token = new JwtSecurityToken(
                claims: claims,
                expires: expire,
                signingCredentials: new SigningCredentials(new SymmetricSecurityKey(secretKeyBytes), SecurityAlgorithms.HmacSha256Signature)
            );

            return tokenHandler.WriteToken(token);
        }
    }
}
