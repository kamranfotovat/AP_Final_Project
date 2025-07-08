using GolestanProject.Models;
using Microsoft.AspNetCore.Mvc;

namespace GolestanProject.Controllers
{
    public class StudentController : Controller
    {

        private readonly MyAppContext _context;
        public StudentController (MyAppContext context)
        {
            _context = context;
        }

        public IActionResult StudentDashboard ()
        {
            var roleSelected = HttpContext.Session.GetString("SelectedRole");
            if (roleSelected != "Student")
            {
                return RedirectToAction("SelectRole", "Account");
            }
            return View();
        }
    }
}
