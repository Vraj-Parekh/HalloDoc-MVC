using Entities.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Repositories.Repository.Interface;

namespace TestApp.Controllers
{
 
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IUserService userService;
        private readonly ICityService cityService;

        public HomeController(ILogger<HomeController> logger,IUserService userService,ICityService cityService)
        {
            _logger = logger;
            this.userService = userService;
            this.cityService = cityService;
        }

        public async Task<IActionResult> Index()
        {

            return View();
        }

        public async Task<IActionResult> UserDataTable(string search,int page = 1, int itemsPerPage = 5)
        {
           Pagination<UsersViewModel>? filteredData = await userService.GetFilteredUsers(search, page, itemsPerPage);
            return PartialView("_Table",filteredData);
        }

        public async Task<IActionResult> AddUser(AddUserViewModel model)
        {
            try
            {
                await userService.AddUserInfo(model);
            }
            catch(Exception e)
            {

            }
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> GetUserDetails(int userId)
        {
            return Json(await userService.GetUserInfo(userId));
        }

        public async Task<IActionResult> EditUser(int userId,AddUserViewModel model)
        {
            try
            {
                await userService.EditUserInfo(userId, model);
            }
            catch(Exception e)
            {

            }
            return RedirectToAction("Index");
        }
        public async Task<IActionResult> DeleteUser(int userId)
        {
            try
            {
                await userService.DeleteUser(userId);
            }
            catch(Exception e)
            {

            }
            return RedirectToAction("Index");
        }
        public IActionResult Privacy()
        {
            return View();
        }

    }
}