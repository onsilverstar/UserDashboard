using UserDashboard.Models;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;
using System.Linq;
using Newtonsoft.Json;

namespace UserDashboard.Controllers
{
    public class AccountController : Controller
    {
        private MyContext dbcontext;
        private readonly SignInManager<User> signInManager;
        private readonly UserManager<User> userManager;
        public AccountController(MyContext context, SignInManager<User> _signInManager, UserManager<User> _userManager)
        {
            dbcontext=context;
            signInManager=_signInManager;
            userManager=_userManager;
            
        }
        [Route("Account/Login")]
        [HttpPost]
        public async Task<IActionResult> ProcessLogin(Login model)
        {
            if (ModelState.IsValid)
            {
                var result=await signInManager.PasswordSignInAsync(model.UserName, model.Password, isPersistent:false, lockoutOnFailure: true);
                if(result.Succeeded)
                {
                    HttpContext.Session.SetObjectAsJson("Username",model.UserName);

                    return RedirectToAction("AllUsers", "Home");
                }
                
                    ModelState.AddModelError(string.Empty, "Invalid Login");
                    ViewBag.LoginError=ModelState.ToList();
                
            }
            var val=ModelState.ToList();
            return View("Login",model);
            
        }
        [Route("Account/AdminLoginPage")]
        [HttpPost]
        public async Task<IActionResult> AdminLoginPage(User model)
        {
            if (ModelState.IsValid)
            {
                var result=await signInManager.PasswordSignInAsync(model.UserName, model.Password, isPersistent:false, lockoutOnFailure: true);
                if(result.Succeeded)
                {

                    return RedirectToAction("AllOrders", "Home", model);
                }
                
                    ModelState.AddModelError(string.Empty, "Invalid Login");
                
            }
            return View();
            
        }
        [Route("Account/Register")]
        [HttpPost]
        public async Task<IActionResult> ProcessRegister(User model)
        {
            if (ModelState.IsValid)
            {
                User newuser=new User {UserName=model.UserName, Description=model.Description, Email=model.Email,EmailConfirmed=model.EmailConfirmed, Password=model.Password, FirstName=model.FirstName, LastName=model.LastName};
                IdentityResult result=await userManager.CreateAsync(newuser, model.Password);
                if(result.Succeeded)
                {
                    if(dbcontext.Users.Count()<1)
                    {
                        await userManager.AddToRoleAsync(newuser, "Level3");
                        return RedirectToAction("Login", model); 
                    }
                    else
                    {
                        await userManager.AddToRoleAsync(newuser, "Level1");
                        return RedirectToAction("Login", model); 
                    }
                }
                foreach(var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
                
            }
            return View("Register", model);
            
        }
        private Task<User> GetCurrentUserAsync()
        {
            return userManager.GetUserAsync(HttpContext.User);
        }
        [Route("Account/Login")]
        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }
        [Route("Account/AdminLogin")]
        [HttpGet]
        public IActionResult AdminLogin(User model)
        {
            return View();
        }
        
    }
    
}