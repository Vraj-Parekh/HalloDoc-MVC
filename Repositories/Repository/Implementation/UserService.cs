using Entities.DataContext;
using Entities.Models;
using Entities.ViewModels;
using Microsoft.EntityFrameworkCore;
using Repositories.Repository.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.Repository.Implementation
{
    public class UserService: IUserService
    {
        private readonly HalloDocDbContext _context;

        public UserService(HalloDocDbContext _context)
        {
            this._context = _context;
        }

        public User GetUserByEmail(string email)
        {
            return _context.Users.Where(a => a.Email == email).FirstOrDefault();
        }

        public async Task<List<PatientHistoryDTO>> GetFilteredUsers(string firstName, string lastName, string email, string phoneNumber)
        {
            IQueryable<User>? query = _context.Users.AsQueryable();

            if(!string.IsNullOrEmpty(firstName) || !string.IsNullOrEmpty(lastName) || !string.IsNullOrEmpty(email) || !string.IsNullOrEmpty(phoneNumber))
            {
                firstName = (firstName!=null) ? firstName.ToLower().Trim() : null;
                lastName = (lastName != null) ? lastName.ToLower().Trim() : null;
                email = (email != null) ? email.ToLower().Trim() : null;
                phoneNumber = (phoneNumber != null) ? phoneNumber.ToLower().Trim() : null;

                query = query.Where(a =>
                    a.Firstname.ToLower().Contains(firstName) ||
                    a.Lastname.ToLower().Contains(lastName) ||
                    a.Email.ToLower().Contains(email) ||
                    a.Mobile.ToLower().Contains(phoneNumber));
            }

            List<User>? users = await query.ToListAsync(); 

            List<PatientHistoryDTO> modelList = new List<PatientHistoryDTO>();

            foreach (var item in users)
            {
                PatientHistoryDTO model = new PatientHistoryDTO()
                {
                    Address = item.City,
                    Email = item.Email,
                    FirstName = item.Firstname,
                    LastName = item.Lastname,
                    PhoneNumber = item.Mobile,
                    UserId = item.Userid
                };
                modelList.Add(model);
            }
            return modelList;
        }
    }
}
