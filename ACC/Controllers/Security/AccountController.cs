﻿using System.Threading.Tasks;
using ACC.ViewModels.Security;
using DataLayer.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace ACC.Controllers.Security
{
    public class AccountController : Controller
    {
        private readonly UserManager<ApplicationUser> UserManager;
        private readonly SignInManager<ApplicationUser> SignInManager;

        public AccountController(UserManager<ApplicationUser> userManager , SignInManager<ApplicationUser> signInManager)
        {
            UserManager = userManager;
            SignInManager = signInManager;
        }
        [HttpGet]
        public IActionResult Register()
        {
            return View("Register");
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterUserVM registerUserVM)
        {
            if (ModelState.IsValid)
            {
                ApplicationUser UserModel = new ApplicationUser();
                UserModel.UserName = registerUserVM.UserName;
                UserModel.PasswordHash = registerUserVM.Password;
                
                IdentityResult Result = await UserManager.CreateAsync(UserModel , registerUserVM.Password);
                
                if (Result.Succeeded)
                {
                    //Create Cookie
                    await SignInManager.SignInAsync(UserModel , false);
                    return RedirectToAction("LogIn", "Account");
                    
                }
                else
                {
                    foreach(var error in Result.Errors)
                    {
                        ModelState.AddModelError("", error.Description);
                        
                    }
                }
            }
          
                return View("Register", registerUserVM);
            
        }

        [HttpGet]
        public IActionResult LogIn()
        {
            return View("LogIn");
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> LogIn(LogInUserVM logInUserVM )
        {
            if(ModelState.IsValid)
            {
                ApplicationUser UserFromDatabase = await UserManager.FindByNameAsync(logInUserVM.UserName);

                if (UserFromDatabase != null)
                {
                    bool IsFound = await UserManager.CheckPasswordAsync(UserFromDatabase, logInUserVM.Password);
                    if (IsFound)
                    {
                        await SignInManager.SignInAsync(UserFromDatabase, logInUserVM.RememberMe);
                        return RedirectToAction("Index", "Home");
                    }
                }
                ModelState.AddModelError("", "Invalid username and password.");
            }

            return RedirectToAction("LogIn", logInUserVM);
        }

        public async Task<IActionResult> LogOut()
        {
            await SignInManager.SignOutAsync();
            return RedirectToAction("LogIn", "Account");
        }
    }
}
