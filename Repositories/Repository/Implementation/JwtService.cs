using Entities.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Repositories.Repository.Interface;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.Repository.Implementation
{
    public class JwtService : IJwtService
    {
        public static string GenerateJwtToken(Aspnetuser user)
        {
            List<Claim>? authClaims = new List<Claim>
            {
                    new Claim("userName", user.Username),
                    new Claim(ClaimTypes.Email, user.Email),
                    new Claim(JwtRegisteredClaimNames.Jti , Guid.NewGuid().ToString()),//unique identifier for the JWT token
                    new Claim("userId", user.Aspnetuserid),
            };
            if (user.Roles.Any(a => a.Name == "Admin"))
            {
                authClaims.Add(new Claim("firstname", user.AdminAspnetusers.FirstOrDefault()?.Firstname ?? ""));
                authClaims.Add(new Claim("lastname", user.AdminAspnetusers.FirstOrDefault()?.Lastname ?? ""));
            }
            if (user.Roles.Any(a => a.Name == "Provider"))
            {
                authClaims.Add(new Claim("firstname", user.PhysicianAspnetusers.FirstOrDefault()?.Firstname ?? ""));
                authClaims.Add(new Claim("lastname", user.PhysicianAspnetusers.FirstOrDefault()?.Lastname ?? ""));
            }
            if (user.Roles.Any(a => a.Name == "Patient"))
            {
                authClaims.Add(new Claim("firstname", user.Users.FirstOrDefault()?.Firstname ?? ""));
                authClaims.Add(new Claim("lastname", user.Users.FirstOrDefault()?.Lastname ?? ""));
            }
            foreach (Aspnetrole role in user.Roles)
            {
                authClaims.Add(new Claim(ClaimTypes.Role, role.Name));

            }

            //Get key from configuration
            SymmetricSecurityKey? key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("7b5e8fb32a9c046b1e4f8dcf02014a307d2f3fe3d820d1f70f9df2f4c6a8b9e5"));

            DateTime expires = DateTime.UtcNow.AddMinutes(60);
            SigningCredentials? creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256Signature);

            //Brought this things from configuration
            JwtSecurityToken? token = new JwtSecurityToken("Issuer", "Audience", authClaims, expires: expires, signingCredentials: creds);



            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        public static bool ValidateToken(string token, out JwtSecurityToken jwtSecurityToken)
        {
            jwtSecurityToken = null;

            if (token == null)
            {
                return false;
            }

            JwtSecurityTokenHandler? tokenHandler = new JwtSecurityTokenHandler();
            SymmetricSecurityKey? key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("7b5e8fb32a9c046b1e4f8dcf02014a307d2f3fe3d820d1f70f9df2f4c6a8b9e5"));

            try
            {
                tokenHandler.ValidateToken(token, validationParameters: new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = key,
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    ClockSkew = TimeSpan.Zero
                }, out SecurityToken validatedToken);

                jwtSecurityToken = (JwtSecurityToken?)validatedToken;

                if (jwtSecurityToken != null)
                {
                    return true;
                }
                return false;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
    }
}
