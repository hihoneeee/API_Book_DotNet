using Microsoft.AspNetCore.Authorization;

namespace TestWebAPI.Settings
{
    public class AuthorizationSetting : IAuthorizationRequirement
    {
        public string Permission { get; private set; }

        public AuthorizationSetting(string permission)
        {
            Permission = permission;
        }
    }
}
