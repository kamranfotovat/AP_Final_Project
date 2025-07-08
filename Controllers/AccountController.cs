using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using GolestanProject.Models;
using BCrypt.Net;
using System.Linq;


namespace GolestanProject.Controllers
{
    public class AccountController : Controller
    {
        private readonly MyAppContext _context;

        public AccountController(MyAppContext context)
        {
            _context = context;
        }

        //      SHOW LOGIN PAGE (GET)
        public IActionResult Login()
        {
            return View();
        }

        //      LOGIN REQUEST (POST)
        [HttpPost]
        public IActionResult Login(string email, string password)
        {
            var user = _context.Users
                .Include(u => u.USERROLES)
                .ThenInclude(ur => ur.ROLE)
                .FirstOrDefault(u => u.email == email);

            if (user != null && BCrypt.Net.BCrypt.Verify(password, user.hashed_password))
            {
                HttpContext.Session.SetInt32("UserId", user.id);
                Console.WriteLine("email and password are ok, now we Redirect to SelectRole");
                return RedirectToAction("SelectRole");
            }
            else
            {
                TempData["ErrorMessage"] = "Invalid email or password";
                Console.WriteLine("password or email are not ok, so we Redirect to Login page");
                return RedirectToAction("Login");
            }
        }

        //      SELECTROLE (GET)
        public IActionResult SelectRole()
        {
            var userId = HttpContext.Session.GetInt32("UserId");
            if (userId == null)
            {
                Console.WriteLine("ERROR: iserId in SelectRole in null");
                return RedirectToAction("Login");
            }

            var roles = _context.User_Roles.Where(a => a.user_id == userId).Select(b => b.ROLE).ToList();
            return View(roles);


            ////      CHECK IF LOGGED IN
            //var userId = TempData["UserId"] as int?;
            //if (userId == null)
            //{
            //    Console.WriteLine("ERROR: userId in SelectRole is null");
            //    return RedirectToAction("Login");
            //}
            ////      GET THE USER ROLES
            //var roles = _context.User_Roles.Where(a => a.user_id == userId)
            //                    .Select(a => a.ROLE).ToList();

            ////      SHOW ROLES IN THE VIEW SO USER CAN SELECT HIS ROLE
            //return View(roles);
            ////      SELECTING ROLE IS HANDLED IN THE RLESELECTED
        }

        //      STORE THE SELECTED ROLE
        [HttpPost]
        public IActionResult RoleSelected(int roleId)
        {
            //      CEHCK IF LOGGED IN
            //var userId = TempData["UserId"] as int?;
            //if (userId == null)
            //{
            //    return RedirectToAction("Login");
            //}

            //var selectedRole = _context.Roles.FirstOrDefault(r => r.id == roleId);
            //if (selectedRole != null)
            //{
            //    TempData["SelectedRole"] = selectedRole.name;

            //    return RedirectToAction("Index", "Home");
            //}
            //else
            //{
            //    TempData["ErrorMessage"] = "Invalid role selected";
            //    return RedirectToAction("SelectRole");
            //}


            var userId = HttpContext.Session.GetInt32("UserId");
            if (userId == null)
            {
                return RedirectToAction("Login");
            }
            // -----------------------------

            var selectedRole = _context.Roles.FirstOrDefault(r => r.id == roleId);
            if (selectedRole != null)
            {
                HttpContext.Session.SetString("SelectedRole", selectedRole.name);

                if (selectedRole.name == "Admin")
                {
                    return RedirectToAction("AdminDashboard", "Admin");
                }
                else if (selectedRole.name == "Instructor")
                {
                    // action needs to be created
                    return RedirectToAction("InstructorDashboard", "Instructor");
                }
                else if (selectedRole.name == "Student")
                {
                    // action needs to be created
                    return RedirectToAction("StudentDashboard", "Student");
                }
                else
                {
                    
                    return RedirectToAction("Index", "Home");
                }
            }
            else
            {
                TempData["ErrorMessage"] = "Invalid role selected";
                return RedirectToAction("SelectRole");
            }


        }

        public IActionResult Logout()
        {
            //      IT CLEARS THE USERID AND REDIRECTS TO LOGIN PAGE
            HttpContext.Session.Clear();
            return RedirectToAction("Login");
        }

        
    }
}