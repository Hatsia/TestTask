using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TestTask.Interfaces;

namespace TestTask.Controllers
{
    public class RoleController : Controller
    {
        private readonly IRoleService _roleService;

        public RoleController(IRoleService roleService)
        {
            _roleService = roleService;
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Index()
        {
            return View(await _roleService.GetRolesAsync());
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> CreateRole(string roleName)
        {
            if (roleName != null)
            {
                await _roleService.CreateRoleAsync(roleName);
            }
            return RedirectToAction("Index");
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteRole(string id)
        {
            if (id != null)
            {
                await _roleService.DeleteRoleByIdAsync(id);
            }
            return RedirectToAction("Index");
        }
    }
}
