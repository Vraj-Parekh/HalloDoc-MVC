﻿using Entities.DataContext;
using Entities.Models;
using Entities.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Repositories.Repository.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.Repository.Implementation
{
    public class AspNetUserService :IAspNetUserService
    {
        private readonly HalloDocDbContext _context;
        private readonly IHttpContextAccessor httpContext;

        public AspNetUserService(HalloDocDbContext _context,IHttpContextAccessor httpContext)
        {
            this._context = _context;
            this.httpContext = httpContext;
        }

        public string AuthenticateUser(LoginDTO data)
        {
            Aspnetuser? user = _context.Aspnetusers
                .Where(u=>u.Email == data.Email)
                .Include(a=>a.Roles)
                .Include(a=>a.Users)
                .Include(a=>a.PhysicianAspnetusers)
                .Include(a=>a.AdminAspnetusers)
                .FirstOrDefault();

            if(user != null && user.Passwordhash == data.Password)
            {
                return JwtService.GenerateJwtToken(user);
            }
            else
            {
                return null;
            }
        }
        public bool isUserPresent(LoginDTO data)
        {
            return _context.Aspnetusers.Any(u => u.Email == data.Email);
        }
        public bool isUserEmailPresent(string email)
        {
            return _context.Aspnetusers.Any(u => u.Email == email);
        }
        public void ChnagePassword(LoginDTO data)
        {
            Aspnetuser? aspNetUserData = new Aspnetuser();
            aspNetUserData.Passwordhash = data.ConfirmPassword;

            _context.Aspnetusers.Update(aspNetUserData);
            _context.SaveChanges();
        }

        public async Task<Aspnetuser> AddAspNetUser(string email, string username, string phone, string password)
        {
            Aspnetuser? user = _context.Aspnetusers.Where(a=>a.Email == email).FirstOrDefault();
            if (user is not null)
            {
                return user;
            }

            Aspnetuser aspnetuser = new Aspnetuser()
            {
                Aspnetuserid = Guid.NewGuid().ToString(),
                Email = email,
                Passwordhash = password,
                Username = username,
                Phonenumber = phone,
                Createddate = DateTime.Now,
            };
            _context.Add(aspnetuser);
            await _context.SaveChangesAsync();
            return aspnetuser;
        }

        public string GetAspNetUserId() => httpContext.HttpContext?.User.FindFirst("userId")?.Value!;
    }
}
