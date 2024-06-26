using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NGOCXMHT_Identity.Models;
using NGOCXMHT_Identity.Models.Process;

namespace NGOCXMHT_Identity.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;

        public AccountController(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;

        }
        
       
        public async Task<ActionResult> Index()
        {
            var users = await _userManager.Users.ToListAsync();
          
        
            return View(users);
        }
        
      
    }
}

