using Microsoft.AspNetCore.Authorization;
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
        [Authorize]
        public IActionResult Index()
        {            
            return View();
        }


        public IActionResult Login()
        {
            
            
            return View();
        }
        public IActionResult Registration()
        {
            return View();
        }

        [HttpPost]       
        public async Task<IActionResult> Login([FromForm] LoginRequest request, string returnUrl)
        {
            returnUrl ??= Url.Content("~/");

            var result = await _userService.LoginAsync(request, HttpContext);

            if (result.Succeeded) 
            {
                return LocalRedirect(returnUrl);
            }
            if (result.IsLockedOut)
            {
                return RedirectToPage("./Lockout");
            }
            else
            {
                //ModelState.AddModelError(string.Empty, "Invalid login attempt.");

                return View();
            }
        }

        public async Task<IActionResult> Logout()
        {
            // удаляем аутентификационные куки
            await _userService.LogoutAsync();
            return RedirectToAction("Index", "Home");
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
