using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Repositories.Repository.Implementation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Primitives;
using System.Linq;

namespace HalloDoc_Project.Attributes
{
    public class CustomAuthorization : Attribute, IAuthorizationFilter
    {
        private readonly List<string> _role;
        

        public CustomAuthorization(string role = "")
        {
            _role = role.Split(',').Select(a => a.Trim()).ToList();
        }
        public void OnAuthorization(AuthorizationFilterContext context)
       {
            if (context.ActionDescriptor.EndpointMetadata.OfType<AllowAnonymousAttribute>().Any())
                return;


            HttpRequest? request = context.HttpContext.Request;

            string? token = request.Cookies["Token"];
            

            if (token == null || !JwtService.ValidateToken(token, out JwtSecurityToken jwtSecurityToken))
            {
                if(_role.Contains("Admin"))
                {
                    context.Result = new RedirectToRouteResult(new RouteValueDictionary(new { controller = "Admin", action = "AdminLogin" }));
                }
                else
                {
                context.Result = new RedirectToRouteResult(new RouteValueDictionary(new { controller = "Patient", action = "PatientLogin" ,returnUrl = request.Path }));
                }
                return;
            }



            List<string>? roleClaim = jwtSecurityToken.Claims.Where(claims => claims.Type == ClaimTypes.Role).Select(a => a.Value).ToList();//extract roleClaim from token

            //Not Logged In
            if (roleClaim is null || !roleClaim.Intersect(_role).Any())
            {
                context.Result = new RedirectToRouteResult(new RouteValueDictionary(new { controller = "Home", action = "AccessDenied" }));
                return;
            }

            IEnumerable<Claim>? claims = jwtSecurityToken.Claims;

            context.HttpContext.User.AddIdentity(new ClaimsIdentity(claims));
            return;
        }
    }
}
