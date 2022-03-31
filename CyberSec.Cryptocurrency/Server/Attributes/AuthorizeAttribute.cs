using CyberSec.Cryptocurrency.Service.Entities;
using CyberSec.Cryptocurrency.Service.Enums;
using CyberSec.Cryptocurrency.Shared.DTOs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace CyberSec.Cryptocurrency.API.Attributes;

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
public class AuthorizeAttribute : Attribute, IAuthorizationFilter
{
    private readonly IList<Role> _roles;

    public AuthorizeAttribute(params Role[] roles)
    {
        _roles = roles ?? Array.Empty<Role>();
    }

    public async void OnAuthorization(AuthorizationFilterContext context)
    {
        // skip authorization if action is decorated with [AllowAnonymous] attribute
        var allowAnonymous = context.ActionDescriptor.EndpointMetadata.OfType<AllowAnonymousAttribute>().Any();
        if (allowAnonymous)
            return;

        // authorization
        var obj = context.HttpContext.Items[nameof(User)];
        var user = obj is not null ? await (Task<UserDto>)obj : null;
        
        if (user is null || (_roles.Any() && !_roles.Contains((Role)user.Role)))
            context.Result = new JsonResult(new { message = "Unauthorized" })
            {
                StatusCode = StatusCodes.Status401Unauthorized
            };
    }
}