using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using database.Models;
using website.Models.Interfaces;
using Microsoft.AspNetCore.Authorization;
using website.Models;

namespace website.Controllers
{
    public class ProfileController : Controller
    {
        private UserManager<User> userManager;
        private SignInManager<User> signInManager;
        private IUMDBRepository db;
        public ProfileController(IUMDBRepository _db, UserManager<User> _userManager, SignInManager<User> _signInManager)
        {
            userManager = _userManager;
            signInManager = _signInManager;
            db = _db;
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Register(UserRegisterModel registerModel)
        {
            if (ModelState.IsValid)
            {
                User user = new User
                {
                    UserName = registerModel.UserName,
                    Email = registerModel.Email,
                    Name = registerModel.Name
                };
                IdentityResult result = await userManager.CreateAsync(user, registerModel.Password);
                if (result.Succeeded)
                {
                    Microsoft.AspNetCore.Identity.SignInResult result1 = await signInManager.PasswordSignInAsync(user, registerModel.Password, false, false);
                    return RedirectToAction("Query", "Search");
                }
                foreach (IdentityError error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }
            }
            return View(registerModel);
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Login(UserLoginModel loginModel)
        {
            if (ModelState.IsValid)
            {
                User user = await userManager.FindByNameAsync(loginModel.UserName);
                if (user != null)
                {
                    await signInManager.SignOutAsync();
                    Microsoft.AspNetCore.Identity.SignInResult result = await signInManager.PasswordSignInAsync(user, loginModel.Password, false, false);
                    return RedirectToAction("Query", "Search");
                }
                else
                {
                    ModelState.AddModelError(nameof(UserLoginModel.UserName), "Неправильне і'мя користувача чи пароль!");
                }
            }
            return View(loginModel);
        }

        [Authorize]
        public async Task<IActionResult> Logout()
        {
            await signInManager.SignOutAsync();
            return RedirectToAction("Query","Search");
        }
    }
}