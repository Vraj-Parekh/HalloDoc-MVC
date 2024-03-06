using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Repositories.Repository.Implementation;
using Microsoft.AspNetCore.Authorization;

namespace HalloDoc_Project.Attributes
{
    public class CustomAuthorization : Attribute, IAuthorizationFilter
    {
        private readonly string _role;

        public CustomAuthorization(string role = "")
        {
            this._role = role;
        }
        public void OnAuthorization(AuthorizationFilterContext context)
        {
            if (context.ActionDescriptor.EndpointMetadata.OfType<AllowAnonymousAttribute>().Any())
                return;
            var request = context.HttpContext.Request;
            var token = request.Cookies["Token"];


            if (token == null || !JwtService.ValidateToken(token, out JwtSecurityToken jwtSecurityToken))
            {
                context.Result = new RedirectToRouteResult(new RouteValueDictionary(new { controller = "Patient", action = "PatientLogin" }));
                return;
            }



            var roleClaim = jwtSecurityToken.Claims.Where(claims => claims.Type == ClaimTypes.Role).Select(a => a.Value).ToList();

            //Not Logged In
            if (roleClaim is null)
            {
                context.Result = new RedirectToRouteResult(new RouteValueDictionary(new { controller = "Patient", action = "PatientLogin" }));
                return;
            }

            if (!roleClaim.Contains(_role))
            {
                context.Result = new RedirectToRouteResult(new RouteValueDictionary(new { controller = "Home", action = "AccessDenied" }));
                return;
            }

            var claims = jwtSecurityToken.Claims;

            context.HttpContext.User.AddIdentity(new ClaimsIdentity(claims));
            return;
        }
    }
}
