using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using GolestanProject.Models;
using BCrypt.Net;
using System.Linq;
using GolestanProject.ViewModels;


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
                Console.WriteLine("Email and password are OK. Redirecting to SelectRole.");
                return RedirectToAction("SelectRole");
            }
            else
            {
                TempData["ErrorMessage"] = "Invalid email or password";
                Console.WriteLine("Password or email are not OK. Redirecting to Login page.");
                return RedirectToAction("Login");
            }
        }


        public IActionResult SelectRole()
        {
            var userId = HttpContext.Session.GetInt32("UserId");
            if (userId == null)
            {
                Console.WriteLine("ERROR: userId in SelectRole GET is null. Redirecting to Login.");
                return RedirectToAction("Login");
            }

            var user = _context.Users
                .Include(u => u.USERROLES)
                    .ThenInclude(ur => ur.ROLE)
                .Include(u => u.STUDENTS)   // Include associated Student accounts
                .Include(u => u.INSTRUCTORS) // Include associated Instructor accounts
                .FirstOrDefault(u => u.id == userId);

            if (user == null)
            {
                Console.WriteLine($"ERROR: User with ID {userId} not found in DB during SelectRole GET. Redirecting to Login.");
                HttpContext.Session.Clear(); // Clear session if user suddenly not found
                return RedirectToAction("Login");
            }

            var model = new SelectRoleViewModel(); // Create an instance of our ViewModel
            int uniqueCounter = 1; // Used to generate unique IDs for each selectable button/option

            // Iterate through user's assigned roles to populate the ViewModel
            foreach (var userRole in user.USERROLES)
            {
                if (userRole.ROLE.name == "Admin")
                {
                    model.AvailableRoleAccounts.Add(new UserRoleAccountDetail
                    {
                        UniqueId = $"admin-{uniqueCounter++}", // Unique ID for form post
                        RoleName = userRole.ROLE.name,
                        AccountId = null, // No specific account ID for Admin
                        DisplayText = "Admin"
                    });
                }
                else if (userRole.ROLE.name == "Student")
                {
                    // Add an entry for each student account associated with the user
                    foreach (var student in user.STUDENTS)
                    {
                        model.AvailableRoleAccounts.Add(new UserRoleAccountDetail
                        {
                            UniqueId = $"student-{student.student_id}", // Use student_id as part of unique ID
                            RoleName = userRole.ROLE.name,
                            AccountId = student.student_id,
                            DisplayText = $"Student (ID: {student.student_id})" // Display text including the ID
                        });
                    }
                    // Handle cases where a user has a 'Student' role but no associated Student entity
                    if (!user.STUDENTS.Any() && user.USERROLES.Any(ur => ur.ROLE.name == "Student"))
                    {
                        model.AvailableRoleAccounts.Add(new UserRoleAccountDetail
                        {
                            UniqueId = $"student-noaccount-{uniqueCounter++}",
                            RoleName = userRole.ROLE.name,
                            AccountId = null, // No specific account ID found
                            DisplayText = "Student (Account Missing)"
                        });
                    }
                }
                else if (userRole.ROLE.name == "Instructor")
                {
                    // Add an entry for each instructor account associated with the user
                    foreach (var instructor in user.INSTRUCTORS)
                    {
                        model.AvailableRoleAccounts.Add(new UserRoleAccountDetail
                        {
                            UniqueId = $"instructor-{instructor.instructor_id}", // Use instructor_id as part of unique ID
                            RoleName = userRole.ROLE.name,
                            AccountId = instructor.instructor_id,
                            DisplayText = $"Instructor (ID: {instructor.instructor_id})" // Display text including the ID
                        });
                    }
                    // Handle cases where a user has an 'Instructor' role but no associated Instructor entity
                    if (!user.INSTRUCTORS.Any() && user.USERROLES.Any(ur => ur.ROLE.name == "Instructor"))
                    {
                        model.AvailableRoleAccounts.Add(new UserRoleAccountDetail
                        {
                            UniqueId = $"instructor-noaccount-{uniqueCounter++}",
                            RoleName = userRole.ROLE.name,
                            AccountId = null, // No specific account ID found
                            DisplayText = "Instructor (Account Missing)"
                        });
                    }
                }
            }

            // Filter out any duplicate entries if a user somehow has multiple identical role-account pairings
            model.AvailableRoleAccounts = model.AvailableRoleAccounts
                .GroupBy(r => new { r.RoleName, r.AccountId }) // Group by role name and account ID
                .Select(g => g.First()) // Take only the first entry from each group (effectively removes duplicates)
                .ToList();

            // If only one role/account is available, automatically select it and redirect
            if (model.AvailableRoleAccounts.Count == 1)
            {
                var soleOption = model.AvailableRoleAccounts.First();
                HttpContext.Session.SetString("SelectedRole", soleOption.RoleName); // Store role name
                if (soleOption.AccountId.HasValue)
                {
                    // Store account ID if available for that role type
                    HttpContext.Session.SetInt32($"Selected{soleOption.RoleName}Id", soleOption.AccountId.Value);
                }
                // Redirect to the appropriate dashboard controller/action (e.g., Index for StudentController)
                return RedirectToAction("Index", soleOption.RoleName);
            }

            return View(model); // Pass the populated ViewModel to the view for display
        }


        [HttpPost]
        public IActionResult SelectRole(SelectRoleViewModel model) // Accepts the ViewModel from the form submission
        {
            var userId = HttpContext.Session.GetInt32("UserId");
            if (userId == null)
            {
                Console.WriteLine("ERROR: userId in SelectRole POST is null. Redirecting to Login.");
                return RedirectToAction("Login");
            }

            // Re-fetch user and their related data again for server-side validation
            var user = _context.Users
                .Include(u => u.USERROLES)
                    .ThenInclude(ur => ur.ROLE)
                .Include(u => u.STUDENTS)
                .Include(u => u.INSTRUCTORS)
                .FirstOrDefault(u => u.id == userId);

            if (user == null)
            {
                Console.WriteLine($"ERROR: User with ID {userId} not found in DB during SelectRole POST. Redirecting to Login.");
                HttpContext.Session.Clear();
                return RedirectToAction("Login");
            }

            // Re-populate the list of available role accounts for server-side validation
            var availableRoleAccountsForValidation = new List<UserRoleAccountDetail>();
            int uniqueCounter = 1; // Must use the same counter logic as the GET action

            foreach (var userRole in user.USERROLES)
            {
                if (userRole.ROLE.name == "Admin")
                {
                    availableRoleAccountsForValidation.Add(new UserRoleAccountDetail
                    {
                        UniqueId = $"admin-{uniqueCounter++}",
                        RoleName = userRole.ROLE.name,
                        AccountId = null,
                        DisplayText = "Admin"
                    });
                }
                else if (userRole.ROLE.name == "Student")
                {
                    foreach (var student in user.STUDENTS)
                    {
                        availableRoleAccountsForValidation.Add(new UserRoleAccountDetail
                        {
                            UniqueId = $"student-{student.student_id}",
                            RoleName = userRole.ROLE.name,
                            AccountId = student.student_id,
                            DisplayText = $"Student (ID: {student.student_id})"
                        });
                    }
                    if (!user.STUDENTS.Any() && user.USERROLES.Any(ur => ur.ROLE.name == "Student"))
                    {
                        availableRoleAccountsForValidation.Add(new UserRoleAccountDetail
                        {
                            UniqueId = $"student-noaccount-{uniqueCounter++}",
                            RoleName = userRole.ROLE.name,
                            AccountId = null,
                            DisplayText = "Student (Account Missing)"
                        });
                    }
                }
                else if (userRole.ROLE.name == "Instructor")
                {
                    foreach (var instructor in user.INSTRUCTORS)
                    {
                        availableRoleAccountsForValidation.Add(new UserRoleAccountDetail
                        {
                            UniqueId = $"instructor-{instructor.instructor_id}",
                            RoleName = userRole.ROLE.name,
                            AccountId = instructor.instructor_id,
                            DisplayText = $"Instructor (ID: {instructor.instructor_id})"
                        });
                    }
                    if (!user.INSTRUCTORS.Any() && user.USERROLES.Any(ur => ur.ROLE.name == "Instructor"))
                    {
                        availableRoleAccountsForValidation.Add(new UserRoleAccountDetail
                        {
                            UniqueId = $"instructor-noaccount-{uniqueCounter++}",
                            RoleName = userRole.ROLE.name,
                            AccountId = null,
                            DisplayText = "Instructor (Account Missing)"
                        });
                    }
                }
            }

            // Remove duplicates for validation purposes
            availableRoleAccountsForValidation = availableRoleAccountsForValidation
                .GroupBy(r => new { r.RoleName, r.AccountId })
                .Select(g => g.First())
                .ToList();

            // Find the selected detail based on the UniqueId submitted by the form
            var selectedDetail = availableRoleAccountsForValidation
                                    .FirstOrDefault(rad => rad.UniqueId == model.SelectedUniqueId);

            if (selectedDetail == null)
            {
                // If the selected UniqueId doesn't match any valid options, it's an error.
                ModelState.AddModelError("", "Invalid role selection. Please try again.");
                // Re-populate the ViewModel with the valid options and return the view for error display.
                model.AvailableRoleAccounts = availableRoleAccountsForValidation; // Re-assign for the view
                return View(model);
            }

            // Store the selected role name in session
            HttpContext.Session.SetString("SelectedRole", selectedDetail.RoleName);

            // If an account ID exists for the selected role, store it in session with a specific key
            if (selectedDetail.AccountId.HasValue)
            {
                HttpContext.Session.SetInt32($"Selected{selectedDetail.RoleName}Id", selectedDetail.AccountId.Value);
            }
            else
            {
                // If no account ID (e.g., Admin role), ensure old IDs for other roles are cleared from session
                HttpContext.Session.Remove("SelectedStudentId");
                HttpContext.Session.Remove("SelectedInstructorId");
            }

            // Redirect to the Index action of the selected role's controller
            if (selectedDetail.RoleName == "Admin")
            {
                return RedirectToAction("AdminDashboard", "Admin");
            }
            else if (selectedDetail.RoleName == "Instructor")
            {
                return RedirectToAction("InstructorDashboard", "Instructor");
            }
            else if (selectedDetail.RoleName == "Student")
            {
                return RedirectToAction("StudentDashboard", "Student");
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }

        // --- LOGOUT ACTION ---
        public IActionResult Logout()
        {
            HttpContext.Session.Clear(); // Clears all data stored in the current user's session
            return RedirectToAction("Login"); // Redirect back to the login page
        }


        //      SELECTROLE (GET)
        //public IActionResult SelectRole()
        //{
        //    var userId = HttpContext.Session.GetInt32("UserId");
        //    if (userId == null)
        //    {
        //        Console.WriteLine("ERROR: iserId in SelectRole in null");
        //        return RedirectToAction("Login");
        //    }

        //    var roles = _context.User_Roles.Where(a => a.user_id == userId).Select(b => b.ROLE).ToList();
        //    return View(roles);


        //    ////      CHECK IF LOGGED IN
        //    //var userId = TempData["UserId"] as int?;
        //    //if (userId == null)
        //    //{
        //    //    Console.WriteLine("ERROR: userId in SelectRole is null");
        //    //    return RedirectToAction("Login");
        //    //}
        //    ////      GET THE USER ROLES
        //    //var roles = _context.User_Roles.Where(a => a.user_id == userId)
        //    //                    .Select(a => a.ROLE).ToList();

        //    ////      SHOW ROLES IN THE VIEW SO USER CAN SELECT HIS ROLE
        //    //return View(roles);
        //    ////      SELECTING ROLE IS HANDLED IN THE RLESELECTED
        //}

        //      STORE THE SELECTED ROLE
        //[HttpPost]
        //public IActionResult RoleSelected(int roleId)
        //{
        //    //      CEHCK IF LOGGED IN
        //    //var userId = TempData["UserId"] as int?;
        //    //if (userId == null)
        //    //{
        //    //    return RedirectToAction("Login");
        //    //}

        //    //var selectedRole = _context.Roles.FirstOrDefault(r => r.id == roleId);
        //    //if (selectedRole != null)
        //    //{
        //    //    TempData["SelectedRole"] = selectedRole.name;

        //    //    return RedirectToAction("Index", "Home");
        //    //}
        //    //else
        //    //{
        //    //    TempData["ErrorMessage"] = "Invalid role selected";
        //    //    return RedirectToAction("SelectRole");
        //    //}


        //    var userId = HttpContext.Session.GetInt32("UserId");
        //    if (userId == null)
        //    {
        //        return RedirectToAction("Login");
        //    }
        //    // -----------------------------

        //    var selectedRole = _context.Roles.FirstOrDefault(r => r.id == roleId);
        //    if (selectedRole != null)
        //    {
        //        HttpContext.Session.SetString("SelectedRole", selectedRole.name);

        //        if (selectedRole.name == "Admin")
        //        {
        //            return RedirectToAction("AdminDashboard", "Admin");
        //        }
        //        else if (selectedRole.name == "Instructor")
        //        {
        //            // action needs to be created
        //            return RedirectToAction("InstructorDashboard", "Instructor");
        //        }
        //        else if (selectedRole.name == "Student")
        //        {
        //            // action needs to be created
        //            return RedirectToAction("StudentDashboard", "Student");
        //        }
        //        else
        //        {

        //            return RedirectToAction("Index", "Home");
        //        }
        //    }
        //    else
        //    {
        //        TempData["ErrorMessage"] = "Invalid role selected";
        //        return RedirectToAction("SelectRole");
        //    }


        //}



        
    }
}