using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using GolestanProject.Models;
using System.Linq;
using System.Collections.Generic;
using Microsoft.AspNetCore.Http;
using BCrypt.Net;
using GolestanProject.ViewModels; // For password hashing
using System.Threading.Tasks; // For async/await operations
using System; // For DateTime operations




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
            return View(new AdminCreateUserViewModel()); // Pass a new, empty ViewModel to the view
        }

        // POST METHOD
        [HttpPost]
        [ValidateAntiForgeryToken] // Recommended for POST actions to prevent XSRF attacks
        public async Task<IActionResult> AdminCreateUser(AdminCreateUserViewModel model)
        {
            if (ModelState.IsValid)
            {
                // Check if a user with this email already exists
                if (await _context.Users.AnyAsync(u => u.email == model.Email))
                {
                    ModelState.AddModelError("Email", "A user with this email already exists.");
                    return View(model); // Return to view with error
                }

                var user = new User
                {
                    first_name = model.FirstName,
                    last_name = model.LastName,
                    email = model.Email,
                    hashed_password = BCrypt.Net.BCrypt.HashPassword(model.Password) // Hash the password
                };

                _context.Users.Add(user);
                await _context.SaveChangesAsync(); // Save the new user to the database

                TempData["SuccessMessage"] = $"User '{user.email}' created successfully!";
                // After successful creation, clear the form by returning a new ViewModel
                return View(new AdminCreateUserViewModel());
            }

            // If ModelState is not valid, return the same view with validation errors
            return View(model);
        }

        public IActionResult AdminManageUsers()
        {

            var users = _context.Users.Include(a => a.USERROLES).ThenInclude(b => b.ROLE).ToList();
            return View(users);
        }

        public IActionResult AdminAssignRole()
        {
            var selectedRole = HttpContext.Session.GetString("SelectedRole");
            if (selectedRole != "Admin")
            {
                return RedirectToAction("SelectRole", "Account");
            }
            return View(new AdminAssignRoleViewModel()); // Pass an empty ViewModel to the view
        }

        // --- ASSIGN ROLE & CREATE ACCOUNT (POST) ---
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AdminAssignRole(AdminAssignRoleViewModel model)
        {
            var selectedRole = HttpContext.Session.GetString("SelectedRole");
            if (selectedRole != "Admin")
            {
                return RedirectToAction("SelectRole", "Account");
            }

            // --- Server-side validation based on selected role ---
            if (model.SelectedRoleName == "Student")
            {
                if (!model.StudentEnrollmentDate.HasValue)
                {
                    ModelState.AddModelError(nameof(model.StudentEnrollmentDate), "Enrollment Date is required for Student role.");
                }
                else if (model.StudentEnrollmentDate.Value > DateTime.Today)
                {
                    ModelState.AddModelError(nameof(model.StudentEnrollmentDate), "Enrollment Date cannot be in the future.");
                }
            }
            else if (model.SelectedRoleName == "Instructor")
            {
                if (!model.InstructorSalary.HasValue)
                {
                    ModelState.AddModelError(nameof(model.InstructorSalary), "Salary is required for Instructor role.");
                }
                else if (model.InstructorSalary.Value < 0)
                {
                    ModelState.AddModelError(nameof(model.InstructorSalary), "Salary cannot be negative.");
                }

                if (!model.InstructorHireDate.HasValue)
                {
                    ModelState.AddModelError(nameof(model.InstructorHireDate), "Hire Date is required for Instructor role.");
                }
                else if (model.InstructorHireDate.Value > DateTime.Today)
                {
                    ModelState.AddModelError(nameof(model.InstructorHireDate), "Hire Date cannot be in the future.");
                }
            }
            // Add other roles here if they have specific requirements and accounts (e.g., Admin doesn't need an account usually)
            else
            {
                ModelState.AddModelError(nameof(model.SelectedRoleName), "Invalid role selected.");
            }

            // Now check overall ModelState validity
            if (ModelState.IsValid)
            {
                // Find the user by email
                var user = await _context.Users
                                         .Include(u => u.USERROLES)
                                             .ThenInclude(ur => ur.ROLE)
                                         .Include(u => u.STUDENTS)
                                         .Include(u => u.INSTRUCTORS)
                                         .FirstOrDefaultAsync(u => u.email == model.TargetUserEmail);

                if (user == null)
                {
                    ModelState.AddModelError(nameof(model.TargetUserEmail), "User with this email not found.");
                    return View(model); // Return to view with error
                }

                // Get the Role entity from the database
                var role = await _context.Roles.FirstOrDefaultAsync(r => r.name == model.SelectedRoleName);
                if (role == null)
                {
                    ModelState.AddModelError(nameof(model.SelectedRoleName), "Selected role is not valid.");
                    return View(model);
                }

                // --- Assign Role if not already assigned ---
                var userRole = user.USERROLES.FirstOrDefault(ur => ur.role_id == role.id);
                if (userRole == null)
                {
                    user.USERROLES.Add(new User_Role { user_id = user.id, role_id = role.id });
                    await _context.SaveChangesAsync();
                }

                // --- Create Account based on Selected Role ---
                if (model.SelectedRoleName == "Student")
                {
                    // Check if a Student account already exists for this user.
                    // This allows for multiple student accounts if your design permits.
                    // If you want ONLY ONE student account per user, you'd check `user.STUDENTS.Any()`.
                    // For now, we'll allow multiple if student_id is a primary key that supports it,
                    // or enforce one if `user_id` is unique in `Students` table.
                    // Assuming you want to allow creating another student account if StudentId isn't provided.
                    // If you want to strictly prevent duplicate student *accounts* linked to the same user:
                    if (user.STUDENTS.Any(s => s.user_id == user.id))
                    {
                        ModelState.AddModelError("", $"User '{user.email}' already has a Student account.");
                        return View(model);
                    }


                    var student = new Student
                    {
                        user_id = user.id,
                        enrollment_date = model.StudentEnrollmentDate.Value // Use .Value because HasValue was checked
                    };
                    _context.Students.Add(student);
                    await _context.SaveChangesAsync();
                    TempData["SuccessMessage"] = $"Student account created and '{model.SelectedRoleName}' role assigned to '{user.email}'.";
                }
                else if (model.SelectedRoleName == "Instructor")
                {
                    // Similar check for instructor account
                    if (user.INSTRUCTORS.Any(i => i.user_id == user.id))
                    {
                        ModelState.AddModelError("", $"User '{user.email}' already has an Instructor account.");
                        return View(model);
                    }

                    var instructor = new Instructor
                    {
                        user_id = user.id,
                        salary = model.InstructorSalary.Value,
                        hire_date = model.InstructorHireDate.Value
                    };
                    _context.Instructors.Add(instructor);
                    await _context.SaveChangesAsync();
                    TempData["SuccessMessage"] = $"Instructor account created and '{model.SelectedRoleName}' role assigned to '{user.email}'.";
                }

                // If successful, clear the form by returning a new, empty ViewModel
                return View(new AdminAssignRoleViewModel());
            }

            // If ModelState is not valid (due to missing required fields for the selected role),
            // return the same view with validation errors.
            return View(model);
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
