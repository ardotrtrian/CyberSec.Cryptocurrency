using CyberSec.Cryptocurrency.Service.Entities;
using CyberSec.Cryptocurrency.Service.Interfaces;

namespace CyberSec.Cryptocurrency.API.Middlewares;

public class JwtMiddleware
{
    private readonly RequestDelegate _next;

    public JwtMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task Invoke(HttpContext context, IUserService userService, IJwtUtils jwtUtils)
    {
        var token = context.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();
        var userId = jwtUtils.ValidateJwtToken(token!);

        if (!string.IsNullOrEmpty(userId))
            context.Items[nameof(User)] = userService.GetAsync(userId);

        await _next(context);
    }
}