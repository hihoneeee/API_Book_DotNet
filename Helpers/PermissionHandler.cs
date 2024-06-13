using Microsoft.AspNetCore.Authorization;
using TestWebAPI.Config;
using TestWebAPI.Data;

namespace TestWebAPI.Helpers
{
    public class PermissionHandler : AuthorizationHandler<AuthorizationConfig>
    {
        private readonly ApplicationDbContext _context;

        public PermissionHandler(ApplicationDbContext context)
        {
            _context = context;
        }

        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, AuthorizationConfig requirement)
        {
            if (context.User.HasClaim(c => c.Type == "permission" && c.Value == requirement.Permission))
            {
                context.Succeed(requirement);
            }
            return Task.CompletedTask;
        }
    }
}
