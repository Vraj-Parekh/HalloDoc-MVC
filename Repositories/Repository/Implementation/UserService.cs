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

        public async Task<Pagination<PatientHistoryDTO>> GetFilteredUsers(string firstName, string lastName, string email, string phoneNumber, int page, int itemsPerPage)
        {
            IQueryable<User>? query = _context.Users.AsQueryable();

            if (!string.IsNullOrEmpty(firstName))
            {
                firstName = firstName.ToLower().Trim();
                query = query.Where(a=>a.Firstname.ToLower().Contains(firstName));
            }
            if (!string.IsNullOrEmpty(lastName))
            {
                lastName = lastName.ToLower().Trim();
                query = query.Where(a=>a.Lastname.ToLower().Contains(lastName));
            }
            if (!string.IsNullOrEmpty(email))
            {
                email = email.ToLower().Trim();
                query = query.Where(a=>a.Email.ToLower().Contains(email));
            }
            if (!string.IsNullOrEmpty(phoneNumber))
            {
                phoneNumber = phoneNumber.ToLower().Trim();
                query = query.Where(a=>a.Mobile.ToLower().Contains(phoneNumber));
            }

            int totalItems = await query.CountAsync();
            int totalPages = (int)Math.Ceiling((double)totalItems / itemsPerPage);

            if (page < 1) page = 1;
            if (page > totalPages) page = totalPages;

            int skip = (page - 1) * itemsPerPage;

            List<User>? users = await query.Skip(skip).Take(itemsPerPage).ToListAsync();

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
            return new Pagination<PatientHistoryDTO>
            {
                Data = modelList,
                TotalPages = totalPages,
                CurrentPage = page
            };
        }
    }
}
