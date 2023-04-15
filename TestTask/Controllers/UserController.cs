using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using TestTask.Interfaces;
using TestTask.Models.RequestModels;
using TestTask.Models.ViewModels;

namespace TestTask.Controllers
{
    public class UserController : Controller
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Registration([FromForm] RegistrationRequest request)
        {
            var result = await _userService.RegistrationAsync(request);
            var userVM = new UserViewModel();            

            if (result == true)
            {
                userVM.Message = "User has been created!";
            }
            else
            {
                userVM.Message = "Error";
            }

            return View(userVM);
        }
    }
}
