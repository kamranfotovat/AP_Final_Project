using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using GolestanProject.Models;
using System.Linq;
using System.Collections.Generic;
using Microsoft.AspNetCore.Http;
using BCrypt.Net; // For password hashing





namespace GolestanProject.Controllers
{
    // dont forget authentication
    public class AdminController : Controller
    {
        private readonly MyAppContext _context;
        public AdminController (MyAppContext context)
        {
            _context = context;
        }

        // GET ADMIN DASHBOARD
        public IActionResult AdminDashboard()
        {
            return View();
        }

        //  GET METHOD
        public IActionResult AdminCreateUser()
        {
            var selectedRole = HttpContext.Session.GetString("SelectedRole");
            if (selectedRole != "Admin")
            {
                return RedirectToAction("SelectRole", "Account");
            }
            return View();
            //var roles = _context.Roles.ToList();
            //ViewBag.Roles = roles;
            //return View(new User());
        }

        // POST METHOD
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AdminCreateUser(AdminCreateUserViewModel model)
        {
            var selectedRole = HttpContext.Session.GetString("SelectedRole");
            if (selectedRole != "Admin")
            {
                return RedirectToAction("SelectRole", "Account");
            }
            if (ModelState.IsValid)
            {
                var newUser = new User
                {
                    first_name = model.FirstName,
                    last_name = model.LastName,
                    email = model.Email,
                    hashed_password = BCrypt.Net.BCrypt.HashPassword(model.Password),
                    created_at = DateTime.Now
                };

                _context.Users.Add(newUser);
                await _context.SaveChangesAsync();

                var role = await _context.Roles.FirstOrDefaultAsync(a => a.name == model.SelectedRoleName);
                if (role == null)
                {
                    ModelState.AddModelError(string.Empty, "Selected role not found");
                    return View(model);
                }

                var userRole = new User_Role
                {
                    user_id = newUser.id,
                    role_id = role.id
                };

                _context.User_Roles.Add(userRole);

                if (model.SelectedRoleName == "Student")
                {
                    if (!model.EnrollmentDate.HasValue)
                    {
                        ModelState.AddModelError("EnrollmentDate", "Enrollment Date is required for student.");
                        return View(model);
                    }
                    var student = new Student
                    {
                        user_id = newUser.id,
                        enrollment_date = model.EnrollmentDate.Value
                    };
                    _context.Students.Add(student);
                }
                else if (model.SelectedRoleName == "Instructor")
                {
                    if (!model.Salary.HasValue)
                    {
                        ModelState.AddModelError("Salary", "Salary is required for Instructor");
                        return View(model);
                    }
                    var instructor = new Instructor
                    {
                        user_id = newUser.id,
                        salary = model.Salary.Value,
                        hire_date = model.HireDate.Value
                    };
                    _context.Instructors.Add(instructor);
                }
                else
                {
                    ModelState.AddModelError("", "Invalid role selected");
                    return View(model);
                }
                await _context.SaveChangesAsync();
                TempData["SuccessMessage"] = $"User {model.Email} created successfully";
                return RedirectToAction("AdminManageUsers");
            }
            return View(model);
        }

        public IActionResult AdminManageUsers()
        {

            var users = _context.Users.Include(a => a.USERROLES).ThenInclude(b => b.ROLE).ToList();
            return View(users);
        }
    }

}


//    users
//namespace GolestanProject.Controllers
//{
//    [Authorize(Roles = "Admin")]
//    public class AdminController : Controller
//    {
//        private readonly MyAppContext _context;

//        public AdminController(MyAppContext context)
//        {
//            _context = context;
//        }

//        //      DASHBOARD, THE MAIN PAGE WHERE WE CHOOSE WHAT TO DO
//            public IActionResult Dashboard()
//        {
//            return View();
//        }

//        //      GET METHOD
//        public IActionResult CreateUser()
//        {
//            var roles = _context.Roles.ToList();
//            ViewBag.Roles = roles;
//            return View();
//        }

//        //      POST METHOD
//        [HttpPost]
//        public IActionResult CreateUser(users user, int roleId)
//        {
//            if (ModelState.IsValid)
//            {
//                user.hashed_password = BCrypt.Net.BCrypt.HashPassword(user.hashed_password);

//                _context.Users.Add(user);
//                _context.SaveChanges();

//                _context.User_Roles.Add(new user_roles
//                {
//                    user_id = user.id,
//                    role_id = roleId
//                });
//                _context.SaveChanges();

//                return RedirectToAction("Dashboard");
//            }
//            return RedirectToAction("CreateUser");
//        }

//        public IActionResult ManageUsers()
//        {
//            var users = _context.Users.Include(a => a.USERROLES)
//                                      .ThenInclude(b => b.ROLE).ToList();
//            return View(users);
//        }

//        //      GET METHOD
//        public IActionResult EditUser(int id)
//        {
//            var user = _context.Users.Include(a => a.USERROLES)
//                                     .ThenInclude(b => b.ROLE).FirstOrDefault();

//            if (user == null)
//            {
//                return NotFound();
//            }

//            var roles = _context.Roles.ToList();
//            ViewBag.Roles = roles;
//            return View(user);
//        }

//        //      POST METHOD
//        [HttpPost]
//        public IActionResult EditUser(users user, int roleId)
//        {
//            if (ModelState.IsValid)
//            {
//                var existingUser = _context.Users.Where(a => a.id == user.id).FirstOrDefault();

//                if (existingUser != null)
//                {
//                    existingUser.first_name = user.first_name;
//                    existingUser.last_name = user.last_name;
//                    existingUser.email = user.email;

//                    if (!string.IsNullOrEmpty(user.hashed_password))
//                    {
//                        existingUser.hashed_password = BCrypt.Net.BCrypt.HashPassword(user.hashed_password);
//                    }

//                    _context.SaveChanges();

//                    var userRole = _context.User_Roles.Where(a => a.user_id == user.id).FirstOrDefault();
//                    if (userRole != null)
//                    {
//                        userRole.role_id = roleId;
//                    }

//                    _context.SaveChanges();

//                    return RedirectToAction("ManageUsers");
//                }
//                return NotFound();
//            }
//            var roles = _context.Roles.ToList();
//            ViewBag.Roles = roles;
//            return View(user);
//        }

//        public IActionResult DeleteUser(int id)
//        {
//            var user = _context.Users.Where(a => a.id == id).FirstOrDefault();
//            if (user == null)
//            {
//                return NotFound();
//            }

//            _context.Users.Remove(user);
//            _context.SaveChanges();

//            return RedirectToAction("ManageUsers");
//        }
//    }
//}
