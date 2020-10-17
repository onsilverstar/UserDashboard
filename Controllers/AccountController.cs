using UserDashboard.Models;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;
using System;
using System.Linq;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Authorization;

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

                    return RedirectToAction("AllUsers", "Account");
                }
                
                    ModelState.AddModelError(string.Empty, "Invalid Login");
                    var LoginError=ModelState.ToList();
                
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
                User newuser=new User {UserName=model.UserName, Description=model.Description, CreatedAt=DateTime.Now, Email=model.Email,EmailConfirmed=model.EmailConfirmed, Password=model.Password, FirstName=model.FirstName, LastName=model.LastName};
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
        [Route("logout")]
        [HttpGet]
        public async Task<IActionResult> Logout()
        {
            await signInManager.SignOutAsync();
            HttpContext.Session.Clear();
            return RedirectToAction("Index", "Home");
        }
        [HttpPost]
        [Route("/update")]
        public async Task<IActionResult> ProcessUpdate(User model)
        {
            if(ModelState.IsValid)
                {
                    User toUpdate= dbcontext.Users.FirstOrDefault(g=>g.Id==HttpContext.Session.GetObjectFromJson<String>("UserViewed"));
                    User newuser=new User {UserName=model.UserName, Description=model.Description, Email=model.Email,EmailConfirmed=model.EmailConfirmed, Password=model.Password, FirstName=model.FirstName, LastName=model.LastName};
                    var role=await userManager.GetRolesAsync(toUpdate);
                    await userManager.RemoveFromRolesAsync(toUpdate, role);
                    await userManager.AddToRoleAsync(toUpdate, model.role[0]);
                    var result=await userManager.RemovePasswordAsync(toUpdate);
                    if(result.Succeeded)
                   {
                        await userManager.AddPasswordAsync(toUpdate, model.Password);
                        dbcontext.Update(toUpdate);
                        dbcontext.SaveChanges();
                        return RedirectToAction("AllUsers");
        
                    }
                    ModelState.AddModelError("Couldnt Update Password, Make sure to use use alphanumeric", model.Password);
                }

           
            return View("EditUser", model);
        }
        [HttpGet]
        [Route("/allusers")]
        [Authorize(Roles="Level1")]
        public async Task<IActionResult> AllUsers()
        {
            List<User> AllUsers= dbcontext.Users.ToList();
            List<User> modellist=new List<User>();
            foreach(var model in AllUsers)
            {
                var role=await userManager.GetRolesAsync(model);
                 User newuser=new User {UserName=model.UserName, Id=model.Id, Description=model.Description, Email=model.Email,EmailConfirmed=model.EmailConfirmed, Password=model.Password, FirstName=model.FirstName, LastName=model.LastName};
                 newuser.role=role;
                 modellist.Add(newuser);
            }
            var currentUser=dbcontext.Users.FirstOrDefault(t=>t.UserName==HttpContext.Session.GetObjectFromJson<string>("Username"));
            string stringrole=(await userManager.GetRolesAsync(currentUser))[0];
            if(stringrole=="Level3")
            {
                return View(modellist);
            }
            return View("AllNonAdmin", modellist);
        }
    
    }
    
}