using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ClinicManagement.Data;
using ClinicManagement.Models;
using Bogus;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ClinicManagement.Areas.Database.Controllers
{
    [Area("Database")]
    [Route("/database-manage/[action]")]
    public class DbManageController : Controller
    {
        private readonly ClinicManagementDbContext _dbContext;
        private readonly UserManager<AppUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        private readonly SignInManager<AppUser> _signinmanager;

        public DbManageController(ClinicManagementDbContext dbContext, UserManager<AppUser> userManager, RoleManager<IdentityRole> roleManager,SignInManager<AppUser> signInManager)
        {
            _dbContext = dbContext;
            _userManager = userManager;
            _roleManager = roleManager;
            _signinmanager=signInManager;
        }

    public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        [Authorize(Roles=RoleName.Administrator)]
        public IActionResult DeleteDb()
        {
            return View();
        }
        
        [TempData]
        public string StatusMessage { get; set;}

        [HttpPost]
        [Authorize(Roles=RoleName.Administrator)]
        public async Task<IActionResult> DeleteDbAsync()
        {
            var success = await _dbContext.Database.EnsureDeletedAsync();

            StatusMessage = success ? "Xóa Database thành công" : "Không xóa được Db";
            
            return RedirectToAction(nameof(Index));
        }
        [HttpPost]
        // [Authorize(Roles=RoleName.Administrator)]
        public async Task<IActionResult> Migrate()
        {
            await _dbContext.Database.MigrateAsync();

            StatusMessage = "Cập nhật Database thành công";
            
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> SeedDataAsync()
        {
            // Create Roles
            var rolenames = typeof(RoleName).GetFields().ToList();
            foreach (var r in rolenames)
            {
                var rolename = (string)r.GetRawConstantValue();
                var rfound = await _roleManager.FindByNameAsync(rolename);
                if (rfound == null)
                {
                    await _roleManager.CreateAsync(new IdentityRole(rolename));
                }
            }

            // admin, pass=admin123, admin@example.com
            var useradmin = await _userManager.FindByEmailAsync("admin");
            if (useradmin == null)
            {
                useradmin = new AppUser()
                {
                    UserName = "admin",
                    Email = "npnhatduy@gmail.com",
                    EmailConfirmed = true,
                };

                await _userManager.CreateAsync(useradmin, "Duy123456@");
                await _userManager.AddToRoleAsync(useradmin, RoleName.Administrator);

                await _signinmanager.SignInAsync(useradmin,false);
                // return RedirectToAction(nameof(SeedDataAsync));
                
            }
            else
            {
                var user=await _userManager.GetUserAsync(this.User);
                if(user==null)
                {
                    return this.Forbid();
                }

                var roles=await _userManager.GetRolesAsync(user);
                if(!roles.Any(r=>r==RoleName.Administrator))
                {
                    return Forbid();
                }
                
            }
            StatusMessage = "Vừa seed Database";
            return RedirectToAction("Index");
        }
    }
}