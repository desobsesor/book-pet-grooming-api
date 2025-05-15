using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace BookPetGroomingAPI.API.Attributes
{
    /// <summary>
    /// Custom attribute for role-based authorization
    /// </summary>
    /// <remarks>
    /// This attribute extends the standard ASP.NET Core authorization attribute
    /// to allow specification of allowed roles
    /// </remarks>
    public class AuthorizeAttribute : Microsoft.AspNetCore.Authorization.AuthorizeAttribute, IAuthorizationFilter
    {
        /// <summary>
        /// Constructor that accepts allowed roles
        /// </summary>
        /// <param name="roles">Allowed roles separated by commas</param>
        public AuthorizeAttribute(string roles = "") : base()
        {
            Roles = roles;
        }

        /// <summary>
        /// Method executed during authorization
        /// </summary>
        /// <param name="context">Authorization context</param>
        public void OnAuthorization(AuthorizationFilterContext context)
        {
            // Check if the action has the [AllowAnonymous] attribute
            var allowAnonymous = context.ActionDescriptor.EndpointMetadata
                .Any(em => em.GetType() == typeof(AllowAnonymousAttribute));

            if (allowAnonymous)
                return;

            // Check if the user is authenticated
            if (context.HttpContext.User?.Identity?.IsAuthenticated != true)
            {
                context.Result = new UnauthorizedResult();
                return;
            }

            // If roles are specified, check that the user has at least one of them
            if (!string.IsNullOrEmpty(Roles))
            {
                var roles = Roles.Split(',');
                var hasRole = roles.Any(role => context.HttpContext.User.IsInRole(role.Trim()));

                if (!hasRole)
                {
                    context.Result = new ForbidResult();
                    return;
                }
            }
        }
    }
}