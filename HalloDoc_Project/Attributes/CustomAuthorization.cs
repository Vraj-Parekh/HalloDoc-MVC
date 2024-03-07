﻿using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Repositories.Repository.Implementation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Primitives;

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


            HttpRequest? request = context.HttpContext.Request;

            string? token = request.Cookies["Token"];
            

            if (token == null || !JwtService.ValidateToken(token, out JwtSecurityToken jwtSecurityToken))
            {
                context.Result = new RedirectToRouteResult(new RouteValueDictionary(new { controller = "Patient", action = "PatientLogin" }));
                return;
            }



            List<string>? roleClaim = jwtSecurityToken.Claims.Where(claims => claims.Type == ClaimTypes.Role).Select(a => a.Value).ToList();//extract roleClaim from token

            //Not Logged In
            if (roleClaim is null || !roleClaim.Contains(_role))
            {
                context.Result = new RedirectToRouteResult(new RouteValueDictionary(new { controller = "Home", action = "AccessDenied" }));
                return;
            }

            ////only partuclar role have access to page like admin patient to their accessible pages
            //if (!roleClaim.Contains(_role))
            //{
            //    return;
            //}

            IEnumerable<Claim>? claims = jwtSecurityToken.Claims;

            context.HttpContext.User.AddIdentity(new ClaimsIdentity(claims));
            return;
        }
    }
}
