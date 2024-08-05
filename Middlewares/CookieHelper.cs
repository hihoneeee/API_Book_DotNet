using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using System.Security.Claims;
using TestWebAPI.Middlewares.Interfaces;


namespace TestWebAPI.Middlewares
{
    public class CookieHelper : ICookieHelper
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public CookieHelper(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task GenerateCookie(int id, string phone, string email, string firstName, string lastName, string roleCode, string avatar, string roleName, DateTime expire)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, id.ToString()),
                new Claim(ClaimTypes.Name, phone),
                new Claim(ClaimTypes.Email, email),
                new Claim("FullName", $"{firstName} {lastName}"),
                new Claim(ClaimTypes.Role, roleCode),
                new Claim("Avatar", avatar),
                new Claim("RoleName", roleName),

            };

            var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            var authProperties = new AuthenticationProperties
            {
                ExpiresUtc = expire,
                IsPersistent = true,
            };

            await _httpContextAccessor.HttpContext.SignInAsync(
                CookieAuthenticationDefaults.AuthenticationScheme,
                new ClaimsPrincipal(claimsIdentity),
                authProperties);
        }
        public string GetUserRole()
        {
            var claimsPrincipal = _httpContextAccessor.HttpContext.User;
            var roleClaim = claimsPrincipal.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Role);
            return roleClaim?.Value;
        }
        public string GetUserAvatar()
        {
            var claimsPrincipal = _httpContextAccessor.HttpContext.User;
            var avatarClaim = claimsPrincipal.Claims.FirstOrDefault(c => c.Type == "Avatar");
            return avatarClaim?.Value;
        }

        public string GetUserFullName()
        {
            var claimsPrincipal = _httpContextAccessor.HttpContext.User;
            var fullNameClaim = claimsPrincipal.Claims.FirstOrDefault(c => c.Type == "FullName");
            return fullNameClaim?.Value;
        }

        public string GetUserRoleName()
        {
            var claimsPrincipal = _httpContextAccessor.HttpContext.User;
            var roleNameClaim = claimsPrincipal.Claims.FirstOrDefault(c => c.Type == "RoleName");
            return roleNameClaim?.Value;
        }
    }
}
