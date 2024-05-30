using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Security.Claims;
using System.Text;
using TestWebAPI.Config;
using TestWebAPI.Repositories;
using TestWebAPI.Repositories.Interfaces;
using static System.Net.Mime.MediaTypeNames;

namespace TestWebAPI.Middlewares
{
    public class JWTHelper : IJWTHelper
    {
        private readonly TokenSettings _tokenSettings;
        private readonly IJwtRepositories _jwtRepositories;

        public JWTHelper(IOptions<TokenSettings> tokenSettings, IJwtRepositories jwtRepositories)
        {
            _tokenSettings = tokenSettings.Value;
            _jwtRepositories = jwtRepositories;
        }

        public async Task<string> GenerateJWTToken(int id, string roleCode, DateTime expire)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var secretKeyBytes = Encoding.UTF8.GetBytes(_tokenSettings.Secret);
            var claims = new[]
            {
                new Claim("id", id.ToString()),
                new Claim("roleCode", roleCode),
                new Claim("iat", DateTimeOffset.UtcNow.ToUnixTimeSeconds().ToString()),
            };

            var token = new JwtSecurityToken(
                claims: claims,
                expires: expire,
                signingCredentials: new SigningCredentials(new SymmetricSecurityKey(secretKeyBytes), SecurityAlgorithms.HmacSha256Signature)
            );

            return tokenHandler.WriteToken(token);
        }

        public async Task<string> GenerateJWTRefreshToken(int id, DateTime expire)
        {
            var tokenHandler = new JwtSecurityTokenHandler();

            var key = Encoding.UTF8.GetBytes(_tokenSettings.Secret);
            var claims = new[]
            {
                new Claim("id", id.ToString()),
            };
            var token = new JwtSecurityToken(
                claims: claims,
                expires: expire,
                signingCredentials: new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            );
            return tokenHandler.WriteToken(token);
        }

        public async Task<bool> ValidateRefreshTokenAsync(string refreshToken)
        {
            if (string.IsNullOrEmpty(refreshToken))
            {
                return false;
            }

            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_tokenSettings.Secret);

            try
            {
                tokenHandler.ValidateToken(refreshToken, new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    ClockSkew = TimeSpan.Zero
                }, out SecurityToken validatedToken);

                var jwtToken = (JwtSecurityToken)validatedToken;

                // Lấy id từ claims
                var idClaim = jwtToken.Claims.FirstOrDefault(x => x.Type == "id");
                if (idClaim == null)
                {
                    return false;
                }

                if (!int.TryParse(idClaim.Value, out int userId))
                {
                    return false;
                }

                // Kiểm tra user_id có tồn tại trong bảng JWT hay không
                var existingJwt = await _jwtRepositories.GetJwtByUserId(userId);
                if (existingJwt == null)
                {
                    return false;
                }

                // Kiểm tra user_id trong refresh token có trùng khớp với user_id trong bảng JWT hay không
                if (existingJwt.user_id != userId)
                {
                    return false;
                }

                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }


        public int GetUserIdFromToken(string token)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_tokenSettings.Secret);

            tokenHandler.ValidateToken(token, new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(key),
                ValidateIssuer = false,
                ValidateAudience = false,
                ClockSkew = TimeSpan.Zero
            }, out SecurityToken validatedToken);

            var jwtToken = (JwtSecurityToken)validatedToken;
            var userIdClaim = jwtToken.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier);

            return userIdClaim != null ? int.Parse(userIdClaim.Value) : 0;
        }

        public string GetUserRoleFromToken(string token)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_tokenSettings.Secret);

            tokenHandler.ValidateToken(token, new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(key),
                ValidateIssuer = false,
                ValidateAudience = false,
                ClockSkew = TimeSpan.Zero
            }, out SecurityToken validatedToken);

            var jwtToken = (JwtSecurityToken)validatedToken;
            var roleClaim = jwtToken.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Role);

            return roleClaim != null ? roleClaim.Value : "";
        }
    }
}
