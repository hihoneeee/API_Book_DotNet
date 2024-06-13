using Microsoft.AspNetCore.Authorization;

namespace TestWebAPI.Config
{
    public class AuthorizationConfig : IAuthorizationRequirement
    {
        public string Permission { get; private set; }

        public AuthorizationConfig(string permission)
        {
            Permission = permission;
        }
    }
}
