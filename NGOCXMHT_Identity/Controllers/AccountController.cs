using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NGOCXMHT_Identity.Models;
using NGOCXMHT_Identity.Models.Process;
using NGOCXMHT_Identity.Models.ViewModels;

namespace NGOCXMHT_Identity.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public AccountController(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
        }
        
       
        public async Task<ActionResult> Index()
        {
            var users = await _userManager.Users.ToListAsync();
            var userWithRoles = new List<UserWithRoleVM>();
            foreach (var user in users){
                var roles = await _userManager.GetRolesAsync(user);
                userWithRoles.Add(new UserWithRoleVM{User = user, Roles = roles.ToList()});
            }
        
            return View(userWithRoles);
        }
        
        public async Task<IActionResult> AssignRole(string userId)
        {   
            AssignRoleVM assignRoleVM= new AssignRoleVM();
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return NotFound();
            }
            var userRoles = await _userManager.GetRolesAsync(user);
            var allRoles = await _roleManager.Roles.Select(r => new RoleVM { Id = r.Id, Name = r.Name }).ToListAsync();
            assignRoleVM = new AssignRoleVM
            {
                UserId = userId,
                AllRoles = allRoles,
                SelectedRoles = userRoles
            };
            return View(assignRoleVM);
        }
        [HttpPost]
        public async Task<IActionResult> AssignRole(AssignRoleVM model)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByIdAsync(model.UserId);
                if (user == null)
                {
                    return NotFound();
                }
                var userRoles = await _userManager.GetRolesAsync(user);
                foreach (var role in model.SelectedRoles)
                {
                    if (!userRoles.Contains(role))
                    {
                        await _userManager.AddToRoleAsync(user, role);
                    }
                }
                foreach (var role in userRoles)
                {
                    if (!model.SelectedRoles.Contains(role))
                    {
                        await _userManager.RemoveFromRoleAsync(user, role);
                    }
                }

                return RedirectToAction("Index", "Account");
            }
            return View(model);
        }

            public async Task<IActionResult> AddClaim(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            var userClaims = await _userManager.GetClaimsAsync(user);
            var model = new UserClaimVM(userId, user.UserName, userClaims.ToList());
            return View(model);
        }
        
        [HttpPost]
        public async Task<IActionResult> AddClaim(string userId, string claimType, string claimValue)
        {
            var user = await _userManager.FindByIdAsync(userId);
            var result = await _userManager.AddClaimAsync(user, new Claim(claimType, claimValue));
            if (result.Succeeded)
            {
                return RedirectToAction("AddClaim", new { userId });
            }
            return View();
        }

        
            public async Task<IActionResult> DeleteClaim(string userId, string claimType, string claimValue)
        {
          var user = await _userManager.FindByIdAsync(userId);
            var claim = (await _userManager.GetClaimsAsync(user)).FirstOrDefault(c => c.Type == claimType && c.Value == claimValue);
            return View(claim);
        }

         [HttpPost]
        public async Task<IActionResult> DeleteClaimComfim(string userId, string claimType, string claimValue)
        {
            var user = await _userManager.FindByIdAsync(userId);
            var claim = (await _userManager.GetClaimsAsync(user)).FirstOrDefault(c => c.Type == claimType && c.Value == claimValue);
            if (claim != null)
            {
                await _userManager.RemoveClaimAsync(user, claim);
            }

            // Chuyển hướng người dùng về trang AddClaim
            return RedirectToAction("AddClaim", new { userId });
        }
    }
}

