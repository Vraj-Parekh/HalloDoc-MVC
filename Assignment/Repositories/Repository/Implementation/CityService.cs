using Entities.DataContext;
using Entities.Models;
using Microsoft.EntityFrameworkCore;
using Repositories.Repository.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.Repository.Implementation
{
    public class CityService : ICityService
    {
        private readonly TestDbContext context;

        public CityService(TestDbContext context)
        {
            this.context = context;
        }

        public async Task<List<City>> GetCities()
        {
            return await context.Cities.ToListAsync();
        }

        public int GetCityIdByName(string name)
        {
            return context.Cities.Where(a => a.Name.ToLower() == name.ToLower()).Select(a => a.Id).FirstOrDefault();
        }
        public string GetCityNameById(int id)
        {
            return context.Cities.Where(a => a.Id == id).Select(a => a.Name).FirstOrDefault();
        }
        public bool IsCityExist(string cityName, out int cityId)
        {
            var flag = context.Cities.Any(a => a.Name.ToLower() == cityName.ToLower());

            if (!flag)
            {
                City city = new City()
                {
                    Name = cityName
                };
                context.Add(city);
                context.SaveChanges();
                cityId = city.Id;
            }
            else
            {
                cityId = GetCityIdByName(cityName);
            }
            return flag;
        }
    }
}
