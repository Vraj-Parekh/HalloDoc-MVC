using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Repositories.Repository.Implementation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Primitives;
using System.Linq;
using Entities.Models;
using Entities.DataContext;
using Microsoft.EntityFrameworkCore;

namespace HalloDoc_Project.Attributes
{
    public class CustomAuthorization : Attribute, IAuthorizationFilter
    {
        private readonly List<string> _menus;


        public CustomAuthorization(string menus = "")
        {
            _menus = menus.Split(',').Select(a => a.Trim()).ToList();
        }
        public void OnAuthorization(AuthorizationFilterContext context)
        {
            if (context.ActionDescriptor.EndpointMetadata.OfType<AllowAnonymousAttribute>().Any())
                return;

            HalloDocDbContext? dbContext = context.HttpContext.RequestServices.GetService(typeof(HalloDocDbContext)) as HalloDocDbContext;

            HttpRequest? request = context.HttpContext.Request;

            string? token = request.Cookies["Token"];


            if (token == null || !JwtService.ValidateToken(token, out JwtSecurityToken jwtSecurityToken))
            {
                context.Result = new RedirectToRouteResult(new RouteValueDictionary(new { controller = "Patient", action = "PatientLogin", returnUrl = request.Path }));

                return;
            }

            if (!_menus.Any(a => !string.IsNullOrEmpty(a)))
            {
                IEnumerable<Claim>? claim = jwtSecurityToken.Claims;

                context.HttpContext.User.AddIdentity(new ClaimsIdentity(claim));
                return;
            }

            var role = jwtSecurityToken.Claims.Where(a => a.Type == ClaimTypes.Role).ToList();
            if (role.Any(a => a.Value == "Patient") && _menus.Contains("Patient"))
            {
                IEnumerable<Claim>? jwtClaim = jwtSecurityToken.Claims;

                context.HttpContext.User.AddIdentity(new ClaimsIdentity(jwtClaim));
                return;
            }

            int roleId = int.Parse(jwtSecurityToken.Claims.Where(claims => claims.Type == "roleId").Select(a => a.Value).First());

            GetRoles(dbContext, roleId, out List<string> menus);

            if (menus is null)
            {
                context.Result = new RedirectToRouteResult(new RouteValueDictionary(new { controller = "Admin", action = "AdminLogin" }));
                return;
            }

            //Roles present and check whether the current page is in its Roles
            if (!menus.Intersect(_menus).Any())
            {
                context.HttpContext.Response.StatusCode = 403;
                context.Result = new RedirectToRouteResult(new RouteValueDictionary(new { controller = "Home", action = "AccessDenied" }));
                return;
            }

            IEnumerable<Claim>? claims = jwtSecurityToken.Claims;

            context.HttpContext.User.AddIdentity(new ClaimsIdentity(claims));
            return;
        }

        public bool GetRoles(HalloDocDbContext context, int roleId, out List<string> menus)
        {
            menus = new();

            Role? role = context.Roles.Where(a => a.Roleid == roleId && a.Isdeleted == false)
                .Include(a => a.Rolemenus)
                .ThenInclude(a => a.Menu)
                .FirstOrDefault();

            if (role is null)
                return false;

            menus = role.Rolemenus.Select(a => a.Menu.Name).ToList();

            return true;
        }
    }
}
