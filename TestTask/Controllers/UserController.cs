using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using System.Threading.Tasks;
using TestTask.Interfaces;
using TestTask.Models.FilterModels;
using TestTask.Models.RequestModels;

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

        [AllowAnonymous]
        public IActionResult Login()
        {
            return View();
        }

        [AllowAnonymous]
        public IActionResult Registration()
        {
            return View();
        }

        [Authorize]
        public IActionResult Delete()
        {
            return View();
        }

        public async Task<IActionResult> GetUsersByFilter()
        {
            return View(await _userService.GetAllUsersFMAsync());
        }

        [HttpPost]
        [AllowAnonymous]
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
                ViewBag.Message = "Wrong name or password";

                return View();
            }
        }

        [Authorize]
        public async Task<IActionResult> Logout()
        {
            await _userService.LogoutAsync();

            return RedirectToAction("Index", "Home");
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Registration([FromForm] RegistrationRequest request)
        {
            var result = await _userService.RegistrationAsync(request);

            if (result == true)
            {
                return Redirect("/User/Login");
            }
            else
            {
                ViewBag.Message = "Credentials are invalid. Try again.";
            }

            return View();
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> Edit()
        {
            var user = await _userService.GetUserByEmailAsync(User.FindFirst(x => x.Type == ClaimTypes.Email).Value);

            var request = new EditUserRequest()
            {
                Email = user.Email,
                FirstName = user.FirstName,
                LastName = user.LastName
            };

            return View(request);
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Edit([FromForm] EditUserRequest request)
        {
            if (ModelState.IsValid)
            {
                var userId = HttpContext.User.FindFirst(x => x.Type == ClaimTypes.NameIdentifier).Value;

                ViewBag.Message = await _userService.EditUserAsync(request);

                await _userService.LogoutAsync();

                return Redirect("/User/Login");
            }

            return View(request);
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Delete(string id)
        {
            var isAdmin = HttpContext.User.IsInRole("admin");
            var userId = HttpContext.User.FindFirst(x => x.Type == ClaimTypes.NameIdentifier).Value;

            if (isAdmin)
            {
                ViewBag.Message = await _userService.DeleteUserByIdAsync(id);

                return View();
            }
            else
            {
                ViewBag.Message = await _userService.DeleteUserByIdAsync(userId);

                await _userService.LogoutAsync();

                return Redirect("/Home/Index");
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetAllAsync()
        {
            return View(await _userService.GetAllUserAsync());
        }

        [HttpPost]
        public async Task<IActionResult> GetUsersByFilter([FromForm] UserFilterModel filter)
        {
            return View(await _userService.GetUsersByFilterAsync(filter));
        }
    }
}
