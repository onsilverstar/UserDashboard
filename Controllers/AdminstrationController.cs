using Microsoft.AspNetCore.Identity;
using System.Linq;
using UserDashboard.Models;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
namespace UserDashboard.Controllers
{
    public class AdminstrationController : Controller
    {
        private MyContext dbcontext;
        private readonly RoleManager<IdentityRole> roleManager;
        
     public AdminstrationController(MyContext context, RoleManager<IdentityRole> _roleManager)
        {
            dbcontext=context;
            roleManager=_roleManager;
        }
        [Route("roles")]
        [HttpGet]
        public IActionResult Roles()
        {
            return View();
        }
        [Route("createroles")]
        [HttpPost]
        public async Task<IActionResult> CreateRole(UserRoles mymodel)
        {
            if(ModelState.IsValid)
            {
                IdentityRole newuser=new IdentityRole{Name=mymodel.RoleName};
                IdentityResult newresult=await roleManager.CreateAsync(newuser);
                if (newresult.Succeeded)
                {
                    return RedirectToAction ("Index", "Home");
                }
                foreach(IdentityError error in newresult.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }
            }
            return View("Roles", mymodel);
        }
    }
    
}