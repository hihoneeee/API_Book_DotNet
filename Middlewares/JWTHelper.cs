using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using TestWebAPI.Config;
using TestWebAPI.Data;
using TestWebAPI.Repositories.Interfaces;

namespace TestWebAPI.Middlewares
{
    public class JWTHelper : IJWTHelper
    {
        private readonly TokenSettings _tokenSettings;
        private readonly IJwtRepositories _jwtRepositories;
        private readonly ApplicationDbContext _context;

        public JWTHelper(IOptions<TokenSettings> tokenSettings, IJwtRepositories jwtRepositories, ApplicationDbContext context)
        {
            _tokenSettings = tokenSettings.Value;
            _jwtRepositories = jwtRepositories;
            _context = context;
        }

        public async Task<string> GenerateJWTToken(int id, string roleCode, DateTime expire)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var secretKeyBytes = Encoding.UTF8.GetBytes(_tokenSettings.Secret);

            // Initialize claims as a List<Claim> to allow dynamic additions
            var claims = new List<Claim>
            {
                new Claim("id", id.ToString()),
                new Claim("roleCode", roleCode),
                new Claim("iat", DateTimeOffset.UtcNow.ToUnixTimeSeconds().ToString()),
            };

            // Fetch user permissions based on the role code
            var userPermissions = await _context.RolePermissions
                                     .Where(rp => rp.codeRole == roleCode)
                                     .Select(rp => rp.Permission.value)  // Assuming Permission.value contains the permission name
                                     .ToListAsync();

            // Add permissions to the claims
            foreach (var permission in userPermissions)
            {
                claims.Add(new Claim("permission", permission));
            }

            // Create token descriptor with the claims
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims.ToArray()),  // Convert List<Claim> to array
                Expires = expire,
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(secretKeyBytes), SecurityAlgorithms.HmacSha256Signature)
            };

            // Create the token
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }

        public async Task<string> GenerateJWTRefreshToken(int id, string roleCode, DateTime expire)
        {
            var tokenHandler = new JwtSecurityTokenHandler();

            var key = Encoding.UTF8.GetBytes(_tokenSettings.Secret);
            var claims = new List<Claim>
            {
                new Claim("id", id.ToString()),
                new Claim("roleCode", roleCode),
                new Claim("iat", DateTimeOffset.UtcNow.ToUnixTimeSeconds().ToString()),
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
            var userIdClaim = jwtToken.Claims.FirstOrDefault(x => x.Type == "id");
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
            var roleClaim = jwtToken.Claims.FirstOrDefault(x => x.Type == "roleCode");
            return roleClaim != null ? roleClaim.Value : "";
        }
    }
}
