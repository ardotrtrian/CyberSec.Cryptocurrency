using CyberSec.Cryptocurrency.Service.Entities;
using Microsoft.AspNetCore.Mvc;

namespace CyberSec.Cryptocurrency.API.Controllers;

[Controller]
public abstract class BaseController : ControllerBase
{
    // returns the current authenticated account (null if not logged in)
    public User User => (User)HttpContext.Items[nameof(User)];
}
