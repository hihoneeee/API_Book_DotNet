using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using System.Security.Claims;
using TestWebAPI.Middlewares.Interfaces;
using TestWebAPI.Repositories.Interfaces;
using TestWebAPI.DTOs.User;


namespace TestWebAPI.Middlewares
{
    public class CookieHelper : ICookieHelper
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IServiceProvider _serviceProvider;

        public CookieHelper(IHttpContextAccessor httpContextAccessor, IServiceProvider serviceProvider)
        {
            _httpContextAccessor = httpContextAccessor;
            _serviceProvider = serviceProvider;

        }

        public async Task GenerateCookie(GetUserDTO userDto, DateTime expire)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, userDto.id.ToString()),
                new Claim(ClaimTypes.Name, userDto.phone),
                new Claim(ClaimTypes.Email, userDto.email),
                new Claim("FullName", $"{userDto.first_name} {userDto.last_name}"),
                new Claim(ClaimTypes.Role, userDto.roleCode),
                new Claim("Avatar", userDto.avatar),
                new Claim("RoleName", userDto.dataRole.value),
            };

            if (userDto.dataRole != null && userDto.dataRole.dataPermission != null)
            {
                foreach (var permission in userDto.dataRole.dataPermission)
                {
                    claims.Add(new Claim("Permissions", permission.value));
                }
            }

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

        public List<string> GetUserPermissions()
        {
            var claimsPrincipal = _httpContextAccessor.HttpContext.User;
            var permissionClaims = claimsPrincipal.Claims
                                                   .Where(c => c.Type == "Permissions")
                                                   .Select(c => c.Value)
                                                   .ToList();
            return permissionClaims;
        }

    }
}
