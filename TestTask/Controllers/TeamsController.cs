using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TestTask.Interfaces;
using TestTask.Models.RequestModels;

namespace TestTask.Controllers
{
    [Authorize]
    public class TeamsController : Controller
    {
        private readonly ITeamService _teamService;

        public TeamsController(ITeamService teamService)
        {
            _teamService = teamService;
        }

        // GET: Teams
        [AllowAnonymous]
        public async Task<IActionResult> Index()
        {
            return View(await _teamService.GetAllTeamsAsync());
        }

        // GET: Teams/Details/5
        [AllowAnonymous]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var team = await _teamService.GetTeamByIdAsync((int)id);

            if (team == null)
            {
                return NotFound();
            }

            return View(team);
        }

        // GET: Teams/Create
        [Authorize(Roles = "Admin")]
        public IActionResult Create()
        {
            return View();
        }

        // POST: Teams/Create
        [HttpPost]
        [Authorize(Roles = "Admin")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(BaseTeamRequest request)
        {
            if (ModelState.IsValid)
            {
                await _teamService.CreateTeamAsync(request);

                return RedirectToAction(nameof(Index));
            }

            return View(request);
        }

        // GET: Teams/Edit/5
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var team = await _teamService.GetTeamByIdAsync((int)id);

            if (team == null)
            {
                return NotFound();
            }

            return View(team);
        }

        // POST: Teams/Edit/5
        [HttpPost]
        [Authorize(Roles = "Admin")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(UpdateTeamRequest request)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    await _teamService.UpdateTeamAsync(request);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!await _teamService.IsTeamExistAsync(request.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }

                return RedirectToAction(nameof(Index));
            }

            return View(request);
        }

        // GET: Teams/Delete/5
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var team = await _teamService.GetTeamByIdAsync((int)id);

            if (team == null)
            {
                return NotFound();
            }

            return View(team);
        }

        // POST: Teams/Delete/5
        [HttpPost, ActionName("Delete")]
        [Authorize(Roles = "Admin")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var isSuccessed = await _teamService.DeleteTeamByIdAsync(id);

            if (isSuccessed)
            {
                TempData.Add("1", "User has been deleted!");
            }

            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> JoinTheTeam(int id)
        {
            var userId = User.FindFirst(x => x.Type == ClaimTypes.NameIdentifier).Value;

            var isSuccessed = await _teamService.AttachUserToTeamAsync(userId, id);

            TempData.Clear();

            if (isSuccessed)
            {
                TempData.Add("1", "Welcome to the team!");
            }
            else
            {
                TempData.Add("1", "You are already on another team!");
            }

            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> LeaveTheTeam()
        {
            var userId = User.FindFirst(x => x.Type == ClaimTypes.NameIdentifier).Value;

            await _teamService.DeleteUserFromTeamAsync(userId);

            return RedirectToAction(nameof(Index));
        }
    }
}
