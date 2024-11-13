using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using StuffyHelper.Common.Exceptions;
using StuffyHelper.Common.Helpers;

namespace StuffyHelper.Common.Web;

/// <summary>
/// Class for authorized API conmtrollers 
/// </summary>
public abstract class AuthorizedApiController : Controller
{
    /// <summary>
    /// Bearer token from request
    /// </summary>
    protected string Token => HttpContext.Request.TryGetToken(out var token) ? token : string.Empty;
    
    /// <summary>
    /// Client identity claims
    /// </summary>
    protected ClaimsIdentity IdentityClaims => HttpContext.User.Identity as ClaimsIdentity ?? throw new
        AuthorizationException("Failed to retrieve client identity");
}