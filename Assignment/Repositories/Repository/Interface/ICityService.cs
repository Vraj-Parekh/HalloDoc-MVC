using Entities.Models;

namespace Repositories.Repository.Interface
{
    public interface ICityService
    {
        Task<List<City>> GetCities();
        string GetCityNameById(int id);
        bool IsCityExist(string cityName, out int cityId);
    }
}