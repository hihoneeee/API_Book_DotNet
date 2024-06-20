using Microsoft.AspNetCore.Authorization;
using TestWebAPI.Data;
using TestWebAPI.Settings;

namespace TestWebAPI.Helpers
{
    public class PermissionHandler : AuthorizationHandler<AuthorizationSetting>
    {
        private readonly ApplicationDbContext _context;

        public PermissionHandler(ApplicationDbContext context)
        {
            _context = context;
        }

        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, AuthorizationSetting requirement)
        {
            if (context.User.HasClaim(c => c.Type == "permission" && c.Value == requirement.Permission))
            {
                context.Succeed(requirement);
            }
            return Task.CompletedTask;
        }
    }
}
