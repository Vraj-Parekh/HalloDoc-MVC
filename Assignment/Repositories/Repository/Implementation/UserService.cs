using Entities.DataContext;
using Entities.Models;
using Entities.ViewModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Repositories.Repository.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace Repositories.Repository.Implementation
{
    public class UserService : IUserService
    {
        private readonly TestDbContext context;
        private readonly ICityService cityService;

        public UserService(TestDbContext _context, ICityService cityService)
        {
            context = _context;
            this.cityService = cityService;
        }

        public async Task<Pagination<UsersViewModel>> GetFilteredUsers(string search, int page, int itemsPerPage)
        {
            IQueryable<User>? users = context.Users.AsQueryable();


            if (!string.IsNullOrEmpty(search))
            {
                search = search.ToLower().Trim();
                users = users.Where(a => a.Firstname.ToLower().Contains(search) || a.Lastname.ToLower().Contains(search));
            }

            int totalItems = await users.CountAsync();
            int totalPages = (int)Math.Ceiling((double)totalItems / itemsPerPage);

            if (page < 1) page = 1;

            int skip = (page - 1) * itemsPerPage;

            List<User>? userRecords = await users
                .Skip(skip)
                .Take(itemsPerPage)
                .ToListAsync();

            List<UsersViewModel> modelList = new List<UsersViewModel>();

            foreach (var item in userRecords)
            {

                UsersViewModel model = new UsersViewModel()
                {
                    UserId = item.Userid,
                    FirstName = item.Firstname,
                    LastName = item.Lastname,
                    Email = item.Email,
                    Age = (int)item.Age,
                    PhoneNumber = item.Phoneno,
                    Gender = item.Gender,
                    City = item.City,
                    Country = item.Country
                };
                modelList.Add(model);
            }

            return new Pagination<UsersViewModel>
            {
                Data = modelList,
                TotalPages = totalPages,
                CurrentPage = page
            };
        }

        public static int CalculateAge(DateTime dob)
        {
            DateTime today = DateTime.Today;
            int age = today.Year - dob.Year;

            return age;
        }

        public static string CheckGender(int gender)
        {
            if (gender == 1)
                return "Male";
            else if (gender == 2)
                return "Female";

            return "Other";
        }


        public async Task AddUserInfo(AddUserViewModel model)
        {
            User user = new User()
            {
                Firstname = model.FirstName,
                Lastname = model.LastName,
                Email = model.Email,
                Age = CalculateAge(model.DateOfBirth),
                Gender = CheckGender(model.Gender),
                Phoneno = model.PhoneNumber,
                Country = model.Country,
            };
            int cityId;
            if (cityService.IsCityExist(model.City, out cityId))
            {
                user.Cityid = cityId;
                user.City = model.City;
            }
            else
            {
                user.Cityid = cityId;
                user.City = cityService.GetCityNameById(cityId);
            }

            context.Add(user);
            await context.SaveChangesAsync();
        }

        public async Task<AddUserViewModel> GetUserInfo(int userId)
        {
            var user = await context.Users.Where(a => a.Userid == userId).FirstOrDefaultAsync();
            if (user is not null)
            {
                AddUserViewModel model = new AddUserViewModel()
                {
                    FirstName = user.Firstname,
                    LastName = user.Lastname,
                    Email = user.Email,
                    PhoneNumber = user.Phoneno,
                    Gender = 1,
                    City = user.City,
                    Country = user.Country,
                    DateOfBirth = DateTime.Now,
                    userId = user.Userid
                };
                return model;
            }
            return new AddUserViewModel();
        }

        public async Task EditUserInfo(int userId, AddUserViewModel model)
        {
            var user = await context.Users.Where(a => a.Userid == userId).FirstOrDefaultAsync();
            if (user is not null)
            {
                user.Age = CalculateAge(model.DateOfBirth);
                user.City = model.City;
                user.Country = model.Country;
                user.Firstname = model.FirstName;
                user.Lastname = model.LastName;
                user.Email = model.Email;
                user.Country = model.Country;
                user.Gender = CheckGender(model.Gender);
                user.Phoneno = model.PhoneNumber;

                context.Update(user);
                await context.SaveChangesAsync();
            }
        }
        public async Task DeleteUser(int userId)
        {
            var user = await context.Users.Where(a => a.Userid == userId).FirstOrDefaultAsync();
            if (user is not null)
            {
                context.Remove(user);
                await context.SaveChangesAsync();
            }
        }
    }
}